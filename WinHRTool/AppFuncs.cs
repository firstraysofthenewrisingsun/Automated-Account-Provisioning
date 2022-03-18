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

        public int sendSMS(string phoneNum, string message)
        {

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
                return 577;
            } else
            {
                return 536;
            }

        }

        public void gmailSMTP(string sender, string receiver, string title, string message)
        {

            var smtpClient = new SmtpClient(Properties.Settings.Default.emailSMTP)
            {
                Port = 587,
                Credentials = new NetworkCredential(Properties.Settings.Default.emailUser, Properties.Settings.Default.emailPass),
                EnableSsl = true,
            };

            smtpClient.SendMailAsync(sender, receiver, title, message); //sends account creation verfication email to HR Jira servicedesk

        }

        public int startBAT()
        {

            Runspace rs = RunspaceFactory.CreateRunspace(); //instantiate PowerShell runspace that will process the New-ADUser module called later
            rs.Open(); //opens the runspace, available to use

            PowerShell ps = PowerShell.Create(); //create PowerShell command object for New-ADUser
            ps.Runspace = rs; //assign runspace to New-ADUser 

            try
            {
               
                ps.AddScript(File.ReadAllText(Properties.Settings.Default.scriptPath));

                ps.Invoke(); //actual execution of New-ADUser


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return 448;
            }

            return 469;
        }


    }
}
