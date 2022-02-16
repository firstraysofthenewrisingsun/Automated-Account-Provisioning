/*
 * Author: Derek Baugh
 * Title: App Functions
 * Description: Holds SMS, SMTP & remote batch file activation. 
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
                from: new Twilio.Types.PhoneNumber(Properties.Settings.Default.twilioNumber),
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

            var smtpClient = new SmtpClient(Properties.Settings.Default.emailSMTP)
            {
                Port = 587,
                Credentials = new NetworkCredential(Properties.Settings.Default.emailUser, Properties.Settings.Default.gmaillAppPass),
                EnableSsl = true,
            };

            smtpClient.SendMailAsync(sender, receiver,title, message); //sends account creation verfication email to HR Jira servicedesk

            smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

            smtpClient.Dispose();


            return beenSent; //flag returned for later processing
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            //In progress...

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
                
                ps.AddScript(File.ReadAllText(Properties.Settings.Default.scriptDirectory));

                ps.AddArgument(Properties.Settings.Default.adUser);
                ps.AddArgument(Properties.Settings.Default.adPass);
                ps.AddArgument(Properties.Settings.Default.adFQDN);
                ps.AddArgument(Properties.Settings.Default.batCMD);

                ps.Invoke(); //actual execution of New-ADUser


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


    }
}
