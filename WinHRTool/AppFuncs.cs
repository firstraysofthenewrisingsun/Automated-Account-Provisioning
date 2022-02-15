/*
 * Author: Derek Baugh
 * Title: App Functions
 * Description: Holds SMS and email creatioons funcs. 
 */

using System;
using System.ComponentModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace WinHRTool
{
    internal class AppFuncs
    {
        private string response = "";
        public AppFuncs()
        {

        }

        public bool sendSMS(string phoneNum, string message)
        {

            bool beenSent = false; //message delivery verification flag

            string accountSid = Properties.Settings.Default.twilioID;
            string authToken = Properties.Settings.Default.twilioToken;

            TwilioClient.Init(accountSid, authToken); //initiate twilio with credentials retrieved from settings

            var sms = MessageResource.Create( //sends the message to number passed into sendSMS()
                body: message,
                from: new Twilio.Types.PhoneNumber("+18508314020"),
                to: new Twilio.Types.PhoneNumber(phoneNum)
            );

            if (sms.Status.Equals("sent"))
            {
                beenSent = true;
            }


            return beenSent; //flag returned for later processing
        }

        public bool gmailSMTP(string sender, string receiver, string title, string message)
        {
            bool beenSent = false; //message delivery verification flag

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("dbaugh@cardinalpeak.com", Properties.Settings.Default.gmaillAppPass),
                EnableSsl = true,
            };

            smtpClient.SendMailAsync(sender, receiver,title, message); //sends account creation verfication email to HR Jira servicedesk

            smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtpClient.Dispose();


            return beenSent; //flag returned for later processing
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            

            if (e.Cancelled)
            {
               response = "cancel";
            }
            if (e.Error != null)
            {
               response = e.Error.ToString();
            }
            else
            {
               response = "sent";
            }

        }

        public void startBAT()
        {

            Runspace rs = RunspaceFactory.CreateRunspace(); //instantiate PowerShell runspace that will process the New-ADUser module called later
            rs.Open(); //opens the runspace, available to use

            PowerShell ps = PowerShell.Create(); //create PowerShell command object for New-ADUser
            ps.Runspace = rs; //assign runspace to New-ADUser 

            try
            {
                //converts the password retrieved from persistant settings to SecureString for processing in PS
                /*var pw = PowerShell.Create().AddCommand("ConvertTo-SecureString")
                .AddParameter("String", Properties.Settings.Default.gcdsPass)
                .AddParameter("AsPlainText")
                .AddParameter("Force")
                .Invoke();

                //creates the credentials object to authenticate PowerShell command using information retrieved from settings
                var credential = PowerShell.Create().AddCommand("New-Object")
                    .AddParameter("TypeName", "System.Management.Automation.PSCredential")
                    .AddParameter("ArgumentList", new object[] { Properties.Settings.Default.adDomain+"\\"+Properties.Settings.Default.gcdsUser, pw[0] }) // String, SecureString
                    .Invoke();

                //New-ADUser PowerShell command in C#
                ps.AddCommand("Invoke-Command").AddParameter("ComputerName", "os-dcpp102.cardinalpeak.com").AddParameter("Credential", credential[0]).
                    AddParameter("ErrorAction", "Stop").AddParameter("ScriptBlock", ScriptBlock.Create("{Invoke-Expression -Command:cmd.exe /c 'C:\\Users\\dkvpn\\Desktop\\gcdssync.bat'}"));*/

                ps.AddScript(File.ReadAllText("..\\psscripts\\runcmdremote.ps1"));

                ps.Invoke(); //actual execution of New-ADUser


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }
}
