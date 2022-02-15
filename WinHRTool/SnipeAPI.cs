/*
 * Author: Derek Baugh
 * Title: Snipe API
 * Description: Snipe Asset Management REST API wrapper class for LUS  
 *  
 *
 */
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WinHRTool
{
    internal class SnipeAPI
    {

        public SnipeAPI()
        {

        }

        public void createSnipeUser(params string[] attributes) 
        {
            try
            {
                var webRequest = WebRequest.CreateHttp(Properties.Settings.Default.snipeURL);

                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.UserAgent = "hrtool";
                    webRequest.Headers.Add("Authorization", "Bearer "+Properties.Settings.Default.snipeKey);
                    
                    webRequest.ContentType = "application/json";

                    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            first_name = attributes[0],
                            last_name = attributes[1],
                            username = attributes[2],
                            email = attributes[3],
                            password = attributes[4],
                            password_confirmation = attributes[4]
                        });

                        streamWriter.Write(json);
                    }



                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();
                            JObject jObject = JObject.Parse(jsonResponse);

                            Console.WriteLine(jsonResponse);

                            foreach (var item in jObject)
                            {
                                

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
