using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VACC.CRM.Sync.Plugins
{
    public static class RestService
    {
        /*
                public string url { get; set; }
                public string method { get; set; }
                public string username { get; set; }
                public string pwd { get; set; }
                public string operationType { get; set; }

                public WebRequest createWebService()
                {
                    //operationType = "GET";
                    WebRequest web = WebRequest.Create(url + "/" + method);
                    web.ContentType = "text/xml";
                    web.Method = operationType;
                }

                public WebRequest createWebService(string url, string method)
                {
                    this.url = url;
                    this.method = method; 
                    return createWebService();
                }

                public WebRequest createWebService(string url, string method, string username, string password)
                {
                    this.url = url;
                    this.method = method;
                    this.username = username;
                    this.pwd = password;

                    return createWebService();
                }
        */
        public static CRMSyncWeb.Result callRestService(string url, string method, string operationType, string username, string password, CRMSyncWeb.Payload payload = null)
        {
            string body = string.Empty;
            CRMSyncWeb.Result result = null;
            string requestURI = url + "/" + method;
            string error = string.Empty;

            try
            {

                switch (operationType.ToUpper())
                {
                    case "GET":
                        requestURI += @"/" + payload.id;
                        break;
                    case "POST":
                        //*/
                        body = serializePayload(payload);
                        /*/
                        if (payload != null)
                        {
                            var dataContactSerializer = new System.Runtime.Serialization.DataContractSerializer(typeof(CRMSyncWeb.Payload));
                            using (var memoryStream = new MemoryStream())
                            {
                                using (var reader = new StreamReader(memoryStream))
                                {
                                    dataContactSerializer.WriteObject(memoryStream, payload);
                                    memoryStream.Position = 0;
                                    body = reader.ReadToEnd();
                                }
                            }

                        }
                        //*/
                        break;
                    case "PUT":
                        requestURI += @"/" + payload.id;
                        body = serializePayload(payload);
                        break;
                    case "DELETE":
                        requestURI += @"/" + payload.id;
                        break;
                }

                WebRequest web = WebRequest.Create(requestURI);
                web.ContentType = "text/xml";
                web.Method = operationType;
                //web.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password)));

                web.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));

                using (var streamWriter = new StreamWriter(web.GetRequestStream()))
                {
                    streamWriter.Write(body);
                }

                //get response
                //HttpWebResponse rep = web.GetResponse() as HttpWebResponse;

                HttpWebResponse rep = null;

                try
                {
                    rep = (HttpWebResponse)web.GetResponse();
                }
                catch (Exception ex)
                {
                    error = rep.StatusCode.ToString();
                }

                //error = "WebRequest - after GetResponse";

                using (Stream stream = rep.GetResponseStream())
                {

                    // error = "WebRequest - before DataContractSerializer";                    
                    var dataContactSerializer = new System.Runtime.Serialization.DataContractSerializer(typeof(CRMSyncWeb.Result));
                    // error = "WebRequest - before DataContractSerializer.ReadObject";
                    result = dataContactSerializer.ReadObject(stream) as CRMSyncWeb.Result;
                    // error = "WebRequest - after DataContractSerializer.ReadObject";
                }

                //error = "WebRequest - after GetResponseStream";

                switch (rep.StatusCode)
                {
                    case HttpStatusCode.OK:
                        break;

                }
            }
            catch (Exception ex)
            {
                result = new CRMSyncWeb.Result { success = false, isClientError = false, errorMessage = error + "\n" + ex.Message };
            }

            return result;
        }

        private static string serializePayload(CRMSyncWeb.Payload payload)
        {
            string body = string.Empty;

            if (payload != null)
            {
                var dataContactSerializer = new System.Runtime.Serialization.DataContractSerializer(typeof(CRMSyncWeb.Payload));
                using (var memoryStream = new MemoryStream())
                {
                    using (var reader = new StreamReader(memoryStream))
                    {
                        dataContactSerializer.WriteObject(memoryStream, payload);
                        memoryStream.Position = 0;
                        body = reader.ReadToEnd();
                    }
                }

            }
            return body;
        }
    }
}
