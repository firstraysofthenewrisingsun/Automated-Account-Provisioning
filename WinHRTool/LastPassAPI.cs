using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
/*
 * Author: Derek Baugh
 * Title: Last Pass API
 * Description: LP REST API wrapper class for LUS  
 */

using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace WinHRTool
{
    internal class LastPassAPI
    {

        public LastPassAPI()
        {

        }

        public bool addLP(params string[] userdata)
        {
          
            try
            {
                var webRequest = WebRequest.CreateHttp(Properties.Settings.Default.lastpassURL);

                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/json";

                    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            cid = Properties.Settings.Default.lastpassID,
                            provhash = Properties.Settings.Default.lastpassKey,
                            cmd = "batchadd",
                            data = new JavaScriptSerializer().Serialize(new
                            {
                                username = userdata[0],
                                fullname = userdata[1],
                                password = userdata[2]
                            })
                            

                        }); 

                        streamWriter.Write(json);
                    }



                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {


                            var jsonResponse = sr.ReadToEnd();

                            Console.WriteLine(jsonResponse);
                            

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


        public bool deleteLP(string email)
        {
            try
            {
                var webRequest = WebRequest.CreateHttp(Properties.Settings.Default.lastpassURL);

                if (webRequest != null)
                {
                    webRequest.Method = "PATCH";
                    webRequest.ContentType = "application/json";

                    using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(new
                        {
                            cid = Properties.Settings.Default.lastpassID,
                            provhash = Properties.Settings.Default.lastpassKey,
                            cmd = "deluser",
                            data = new JavaScriptSerializer().Serialize(new
                            {
                                username = "hrapp@cardinalpeak.com",
                                deleteaction = "0"
                            })


                        });

                        streamWriter.Write(json);
                    }



                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {


                            var jsonResponse = sr.ReadToEnd();

                            Console.WriteLine(jsonResponse);


                        }
                    }

                    using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                    {
                        // Do your processings here....
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
