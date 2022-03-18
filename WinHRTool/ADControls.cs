/*
 * Author: Derek Baugh
 * Title: Active Directory Controls
 * Description: Holds all functions pertaining to Active Directory user account management. Utilized by HRTool Form 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Windows.Forms;
using System.Web.Security;
using System.Collections;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace WinHRTool
{
    internal class ADControls
    {
        private AppFuncs appFuncs;
        private HarvestAPI harvestAPI;
        private LastPassAPI lastPassAPI;
        public ADControls()
        {
            appFuncs = new AppFuncs();
            harvestAPI = new HarvestAPI();
            lastPassAPI = new LastPassAPI();
        }

        public string[] retrieveAD(string searchedUser)
        {
            string[] data = new string[5]; //string array to hold AD user object data
            try
            {


                string DomainPath = "LDAP://"+Properties.Settings.Default.adPDC+"."+Properties.Settings.Default.adDomain+"/"+Properties.Settings.Default.adPath;
                DirectoryEntry searchRoot = new DirectoryEntry(DomainPath); //sets path of OU being searched
                searchRoot.Username = Properties.Settings.Default.adDomain+"\\"+Properties.Settings.Default.adUser; //username of DA acct
                searchRoot.Password = Properties.Settings.Default.adPass; //psswd of DA acct

                DirectorySearcher search = new DirectorySearcher(searchRoot); //AD search module from .NET, starts at aforementioned domain path

                search.Filter = String.Format("(&(objectCategory=person)(anr={0}))", searchedUser); //only show account objects listed as 'person' with the name passed to retrieveAD(), can search by last, full and SamAccount name

                SearchResult resultCol = search.FindOne(); //search result as an object for manipulation

                foreach (PropertyValueCollection key in resultCol.GetDirectoryEntry().Properties.Values) //goes through each property attribute set on the AD user object
                {
                    Console.WriteLine(key.PropertyName+": "+key.Value);

                    switch (key.PropertyName) //assigning property data to string array
                    {
                        case "givenName":

                            data[0] = key.Value.ToString();

                            break;

                        case "sn":

                            data[1] = key.Value.ToString();

                            break;
                        case "userPrincipalName":

                            data[2] = key.Value.ToString();

                            break;
                        case "sAMAccountName":

                            data[3] = key.Value.ToString();

                            break;
                        case "department":

                            data[4] = key.Value.ToString();

                            break;

                    }

                }

            } catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    string[] error = new string[5];
                    error[0] = ex.GetType().ToString();

                    return error;
                }
            }

            return data; //returns the array with attribute properties for later processing
           
        }

        public void retrieveAllAD()
        {
          /* 
           * Retrieves all ad users and list their attributes.
           * Search by last, full and SamAccountName.
           * In progress...
           */

            List<string[]> allUsers = new List<string[]>();
            List<SearchResult> results = new List<SearchResult>();
            
            List<ResultPropertyCollection> rows = new List<ResultPropertyCollection>();
            List<ICollection> values = new List<ICollection>();

            string DomainPath = "LDAP://"+Properties.Settings.Default.adPDC+"."+Properties.Settings.Default.adDomain+"/"+Properties.Settings.Default.adPath;
            DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);
            searchRoot.Username = Properties.Settings.Default.adDomain+"\\"+Properties.Settings.Default.adUser;
            searchRoot.Password = Properties.Settings.Default.adPass;
            DirectorySearcher search = new DirectorySearcher(searchRoot);
            search.Filter = String.Format("(&(objectCategory=person))");

            
            SearchResultCollection resultCol = search.FindAll();
            string[] data = new string[4];

            if (resultCol != null)
            {
               foreach (SearchResult result in resultCol)
                {
                    foreach (string propName in result.Properties.PropertyNames)
                    {
                        foreach (object myCollection in result.Properties[propName])
                        {
                          
                              Console.WriteLine(propName + " : " + myCollection.ToString());

                        }
                    }
                }
            }

        }

        public int AddADUser(params TextBox[] textBoxes)
        {
            /* 
             * Wrapper for PS command New-ADUser
             * Sends onboarding SMS to new users number with AD login info & creation notification emmail to CPIT HR servicedesk 
             */

            bool isContractor;

            Runspace rs = RunspaceFactory.CreateRunspace(); //instantiate PowerShell runspace that will process the New-ADUser module called later
            rs.Open(); //opens the runspace, available to use

            PowerShell ps = PowerShell.Create(); //create PowerShell command object for New-ADUser
            ps.Runspace = rs; //assign runspace to New-ADUser 

            try
            {
                //converts the password retrieved from persistant settings to SecureString for processing in PS
                var pw = PowerShell.Create().AddCommand("ConvertTo-SecureString") 
                .AddParameter("String", Properties.Settings.Default.adPass)
                .AddParameter("AsPlainText")
                .AddParameter("Force")
                .Invoke();

                //creates the credentials object to authenticate PowerShell command using information retrieved from settings
                var credential = PowerShell.Create().AddCommand("New-Object") 
                    .AddParameter("TypeName", "System.Management.Automation.PSCredential")
                    .AddParameter("ArgumentList", new object[] { Properties.Settings.Default.adDomain+"\\"+Properties.Settings.Default.adUser, pw[0] }) // String, SecureString
                    .Invoke();

                string password = Membership.GeneratePassword(10, 3); //generates random, cryptographically secure password using the .NET Security API

                
                var newPW = PowerShell.Create().AddCommand("ConvertTo-SecureString")
                .AddParameter("String", password)
                .AddParameter("AsPlainText")
                .AddParameter("Force")
                .Invoke();

                char[] arr = textBoxes[0].Text.Take(1).ToArray(); //retrieves initial of first name

                char v = Char.ToLower(arr[0]); //converts initial to lower case

                string username = v + textBoxes[1].Text.ToLower(); //creates username according to policy

                string email = username+"@cardinalpeak.com"; //creates email according to policy

                //New-ADUser PS command in C#
                ps.AddCommand("New-ADUser").AddParameter("GivenName", textBoxes[0].Text).AddParameter("SurName", textBoxes[1].Text).AddParameter("Name", textBoxes[0].Text+" "+textBoxes[1].Text)
                    .AddParameter("EmailAddress", email).AddParameter("MobilePhone", textBoxes[2].Text)
                    .AddParameter("Title", textBoxes[3].Text).AddParameter("description", textBoxes[4].Text).AddParameter("UserPrincipalName", username).AddParameter("SamAccountName", username).AddParameter("Accountpassword", newPW[0])
                    .AddParameter("Enabled", 1).AddParameter("Path", Properties.Settings.Default.adPath)
                    .AddParameter("Server", Properties.Settings.Default.adPDC).AddParameter("Credential", credential[0]);

                //executes New-ADUser
                ps.Invoke();

                //runs GCDS sync from batch script on os-dcpp102
                int psScriptResult = appFuncs.startBAT();
                if (psScriptResult == 448)
                {
                    rs.Close();
                    return psScriptResult;
                }

                //creates Last Pass
                bool lastPassCreated = lastPassAPI.addLP(email, textBoxes[0]+" "+textBoxes[1], password);

                if (lastPassCreated == false)
                {
                    rs.Close();
                    return 825;
                }

                //creates Harvest
                if (textBoxes[4].Text.Equals("Contractor"))
                {
                    isContractor = true;
                } else
                {
                    isContractor = false;
                }

                string harvestID = harvestAPI.createHarvestUser(textBoxes[0].Text, textBoxes[1].Text, email, isContractor);

                if (harvestID == null)
                {
                    rs.Close();
                    return 305;
                }

                //assigns Harvets user ID to AD account under department attribute
                TextBox info = new TextBox();
                info.Tag = "department";
                info.Text = harvestID;
                List<TextBox> edits = new List<TextBox>();
                edits.Add(info);
                
                editADUser(username, false, edits);

                string message = "Welcome to Cardinal Peak!\nCongratulations on your new position! We're excited to have you join the team\nAs part of our automated onboarding process you'll receive this text message and an account setup email for your Cardinal Peak Google account\n" +
                    "Your credentials for your Active Directory account are as follows\nUsername: "+username+"\n"+"Password: "+password+"\n"+"Please use these credentials to login into the VPN and authenticate actions on your computer";

                //sends onboarding sms to new user at specified number.
                int smsVerification = appFuncs.sendSMS(textBoxes[2].Text, message);

                if (smsVerification == 536)
                {
                    rs.Close();
                    return smsVerification;
                }

                appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailRecipient, "Accounts for new user "+username+"", "New accounts for "+username+" have been created!"); //sends account verfification to notification email entered during initial setup

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                rs.Close();
                return 302;
            }

            rs.Close();

            return 300;
        }

        public int editADUser(string user, bool operationParam, List<TextBox> textBoxes)
        {

          /* 
           * Wrapper for PS command Set-ADUser.
           * Sets attributes specified by user (only a select few are accessible).
           */

            Dictionary<string, string>harvestAttributes = new Dictionary<string, string>();

            bool harvestComplete;

            Runspace rs = RunspaceFactory.CreateRunspace(); //same PowerShell initialization as addADUser()
            rs.Open();

            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;
            //ps.AddCommand("Get-Command");

            try
            {
                //converts the password retrieved from persistant settings to SecureString for processing in PS
                var pw = PowerShell.Create().AddCommand("ConvertTo-SecureString")
                .AddParameter("String", Properties.Settings.Default.adPass)
                .AddParameter("AsPlainText")
                .AddParameter("Force")
                .Invoke();

                //creates the credentials object to authenticate PowerShell command using information retrieved from settings
                var credential = PowerShell.Create().AddCommand("New-Object")
                    .AddParameter("TypeName", "System.Management.Automation.PSCredential")
                    .AddParameter("ArgumentList", new object[] { Properties.Settings.Default.adDomain+"\\"+Properties.Settings.Default.adUser, pw[0] }) // String, SecureString
                    .Invoke();
                

                foreach (TextBox txt in textBoxes)
                {
                    ps.AddCommand("Set-ADUser").AddParameter("Identity", user).AddParameter("Server", Properties.Settings.Default.adPDC).AddParameter("Credential", credential[0]).AddParameter(txt.Tag.ToString(), txt.Text); //changes the attributes selected byt the operator. attribute names are tagged to textbox object for selection
                    ps.Invoke();

                    if (txt.Tag.ToString() == "GivenName")
                    {
                        harvestAttributes.Add("first_name", txt.Text);
                    }

                    if (txt.Tag.ToString() == "SurName")
                    {
                        harvestAttributes.Add("last_name", txt.Text);
                    }

                    if (txt.Tag.ToString() == "EmailAddress")
                    {
                        harvestAttributes.Add("email", txt.Text);
                    }

                }


            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    rs.Close();
                    return 202;
                }
   
            }

                
            //runs GCDS sync from batch script on os-dcpp102
                int psScriptResult = appFuncs.startBAT();
                if (psScriptResult == 448)
                {
                    rs.Close();
                    return psScriptResult;
                }




             //updates Last Pass


            //updates Harvest if flag raised
            if (operationParam != false)
            {
                string[] userAttributes = retrieveAD(user);

                harvestComplete = harvestAPI.editHarvestUser(userAttributes[4], harvestAttributes);

                if (harvestComplete == false)
                {
                    rs.Close();

                    return 205;
                }
            }

            string message = "Changes made";
            //sends account verfification to notification email entered during initial setup
            appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailRecipient, "Edits to "+user, message);

            rs.Close();

            return 200;
        }

        public int deleteUsers(TextBox user)
        {
            /* 
             * Deletes specified AD user object & assosciated accounts 
             */

            try
            {
                string DomainPath = "LDAP://"+Properties.Settings.Default.adPDC+"."+Properties.Settings.Default.adDomain+"/"+Properties.Settings.Default.adPath;
                DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);
                searchRoot.Username = Properties.Settings.Default.adDomain+"\\"+Properties.Settings.Default.adUser;
                searchRoot.Password = Properties.Settings.Default.adPass;
                DirectorySearcher search = new DirectorySearcher(searchRoot);
                search.Filter = String.Format("(&(objectCategory=person)(anr={0}))", user.Text);

                SearchResult resultCol = search.FindOne();

                //deletes AD object
                resultCol.GetDirectoryEntry().DeleteTree();  

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return 102;
            }

            //runs GCDS sync from batch script on os-dcpp102
            int psScriptResult = appFuncs.startBAT();
            if (psScriptResult == 448)
            {
                return psScriptResult;
            }

            string harvestID = "";

            //archives Harvest account
            bool harvestArchiveComplete = harvestAPI.archiveHarvestUser(harvestID); 

            if (harvestArchiveComplete == false)
            {
                return 105;
            }

            string message = "Account for "+user+" deleted!";

            //sends notification email to hr it servicedesk
            appFuncs.gmailSMTP(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailRecipient, "Account for "+user+ " deleted", message); 

            return 100;
        }

       

    }
}


