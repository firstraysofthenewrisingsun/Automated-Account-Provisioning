/*
 * Author: Derek Baugh
 * Title: Credential Protection
 * Description: Encrypts passwords using the Data Protection API available in .NET.
 * Saves the encrypted credentials to the Application Settings for persistant availability between sessions.
 * Decrypts passwords using the same API.
 *  
 *
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.ClientServices.Providers;
using System.Windows;

namespace WinHRTool
{
    internal class CredProtect
    {
        private static byte[] s_additionalEntropy = { 9, 8, 7, 6, 5 };

        public CredProtect()
        {

        }


        public byte[] Encrypt(string OPT, byte[] data)
        {
            try
            {
                              
                byte[] encryption = ProtectedData.Protect(data, s_additionalEntropy, DataProtectionScope.CurrentUser); //encryption via Data Protection class call
                string encrypted = System.Convert.ToBase64String(encryption); //convert byte array containing encrypted password to string for storage in app settings
                

                switch (OPT)
                {
                    case "AD":

                        Properties.Settings.Default.adPass = encrypted; //if operation flag AD set encrypted AD password

                        break;
                    case "EMAIL":

                        Properties.Settings.Default.emailPass = encrypted; //if operation flag EMAIL set encrypted email password

                        break;
                }

                Properties.Settings.Default.Save(); //saves the appropriate properties

                return encryption; //result returned for notification

            }
            catch (Exception ex)
            {

                return null;

            }
        }

        public byte[] Decrypt(byte[] data)
        {
            try
            {
                //decryption via Data Protection class call
                return ProtectedData.Unprotect(data, s_additionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {

                return null;
            }
        }
    }

   
}
