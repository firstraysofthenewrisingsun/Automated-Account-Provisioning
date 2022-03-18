/*
 * Author: Derek Baugh
 * Title: Harvest API
 * Description: Harvest REST API wrapper class for LUS  
 */
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WinHRTool
{
    internal class HarvestAPI
    {

        public HarvestAPI()
        {

        }

        public string createHarvestUser(string fname, string lname, string email, bool isContractor)
        {
            string id = "";
            try
            {
                var webRequest = WebRequest.CreateHttp(Properties.Settings.Default.harvestURL);

                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.UserAgent = "hrtool";
                    webRequest.Headers.Add("Authorization", Properties.Settings.Default.harvestKey);
                    webRequest.Headers.Add("Harvest-Account-ID", Properties.Settings.Default.harvestID);
                    webRequest.ContentType = "application/json";

                    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            first_name = fname,
                            last_name = lname,
                            email = email,
                            is_active = true,
                            roles = new string[] {"Bench", "Holiday", "PTO"},
                            is_contractor = isContractor

                        });

                        streamWriter.Write(json);
                    }

                 
                   
                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {

                            
                            var jsonResponse = sr.ReadToEnd();
                            JObject jObject = JObject.Parse(jsonResponse);

                            

                            foreach (var item in jObject)
                            {
                                if (item.Key == "id")
                                {
                                    id = item.Value.ToString();

                                    Console.WriteLine(id);

                                    
                                }
                                
                            }
                           
                        }
                    }
                }
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.ToString());

                return null;
                
            }
            

            return id;
        }

        public bool editHarvestUser(string id, Dictionary<string, string> changedAttributes)
        {

            try
            {
                var webRequest = WebRequest.CreateHttp(Properties.Settings.Default.harvestURL+id);

                if (webRequest != null)
                {
                    webRequest.Method = "PATCH";
                    webRequest.UserAgent = "hrtool";
                    webRequest.Headers.Add("Authorization", Properties.Settings.Default.harvestKey);
                    webRequest.Headers.Add("Harvest-Account-ID", Properties.Settings.Default.harvestID);
                    webRequest.ContentType = "application/json";


                    var output = Newtonsoft.Json.JsonConvert.SerializeObject(changedAttributes);
                                  
                    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        streamWriter.Write(output);
                    }


                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();
                            JObject jObject = JObject.Parse(jsonResponse);

                           

                            foreach (var item in jObject)
                            {
                               
                                    Console.WriteLine(item);

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                
                return false;
            }

            return true;

        }

        public bool archiveHarvestUser(string id)
        {

            try
            {
                var webRequest = WebRequest.CreateHttp(Properties.Settings.Default.harvestURL+id);

                if (webRequest != null)
                {
                    webRequest.Method = "PATCH";
                    webRequest.UserAgent = "hrtool";
                    webRequest.Headers.Add("Authorization", Properties.Settings.Default.harvestKey);
                    webRequest.Headers.Add("Harvest-Account-ID", Properties.Settings.Default.harvestID);
                    webRequest.ContentType = "application/json";

                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    headers.Add("is_active", "false");


                    var output = Newtonsoft.Json.JsonConvert.SerializeObject(headers);

                    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        streamWriter.Write(output);
                    }


                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();
                            JObject jObject = JObject.Parse(jsonResponse);

                            foreach (var item in jObject)
                            {
                                if (item.Key == "id")
                                {
                                    Console.WriteLine(item.Value);

                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

            return true;

        }

    }
}
