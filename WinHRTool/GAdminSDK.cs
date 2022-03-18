using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinHRTool
{
    internal class GAdminSDK
    {
        private static string[] Scopes = { DirectoryService.Scope.AdminDirectoryUserReadonly };
        private static string ApplicationName = "Directory API .NET Quickstart";
        public GAdminSDK()
        {

        }

        public void listUsers()
        {
            GoogleCredential credential;

            using (var stream =
                new FileStream(Properties.Settings.Default.googleAdminJSON, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                credential = GoogleCredential.FromStream(stream).CreateScoped(DirectoryService.Scope.AdminDirectoryUser);
            }

            // Create Directory API service.
            var service = new DirectoryService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            UsersResource.ListRequest request = service.Users.List();
            request.Customer = "my_customer";
            request.MaxResults = 10;
            request.OrderBy = UsersResource.ListRequest.OrderByEnum.Email;

            // List users.
            IList<User> users = request.Execute().UsersValue;
            Console.WriteLine("Users:");
            if (users != null && users.Count > 0)
            {
                foreach (var userItem in users)
                {
                    Console.WriteLine("{0} ({1})", userItem.PrimaryEmail,
                        userItem.Name.FullName);
                }
            }
            else
            {
                Console.WriteLine("No users found.");
            }
            Console.Read();
        }
    }
}
