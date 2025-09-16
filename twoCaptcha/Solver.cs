using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace twoCaptcha
{  
    public  class Solver
    {
        public string get(string id)
        {
           string key = "142286423b9c59c950c8239a350e11a6";
            string html = string.Empty;
            string url = @"http://2captcha.com/res.php?key=" + key + "&action=get&id="+ id;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return html = reader.ReadToEnd();
            }
        }
        public string post(string googlekey)
        {
            string requestUrl = "http://2captcha.com/in.php";
            HttpWebRequest request = HttpWebRequest.CreateHttp(requestUrl);
            request.Method = "POST";

            // Optionally, set properties of the HttpWebRequest, such as:
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.ContentType = "application/x-www-form-urlencoded";
            // Could also set other HTTP headers such as Request.UserAgent, Request.Referer,
            // Request.Accept, or other headers via the Request.Headers collection.

            // Set the POST request body data. In this example, the POST data is in 
            // application/x-www-form-urlencoded format.  

            string key = "142286423b9c59c950c8239a350e11a6";
            string method = "userrecaptcha";
            string pageurl = "https://www.igssgt.org/cuotas/";
            string postData = "key=" + key + "&method=" + method + "&googlekey=" + googlekey + "&pageurl=" + pageurl;
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postData);
            }

            // Submit the request, and get the response body from the remote server.
            string responseFromRemoteServer;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return responseFromRemoteServer = reader.ReadToEnd();
                }
            }
        }

        public string postHcaptcha(string siteKey, string pageUrl)
        {
            string requestUrl = "http://2captcha.com/in.php";
            HttpWebRequest request = HttpWebRequest.CreateHttp(requestUrl);
            request.Method = "POST";
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.ContentType = "application/x-www-form-urlencoded";

            string key = "142286423b9c59c950c8239a350e11a6"; // Tu API Key de 2Captcha
            string method = "hcaptcha";
            string postData = $"key={key}&method={method}&sitekey={siteKey}&pageurl={pageUrl}";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postData);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd(); // Respuesta esperada: "OK|captcha_id"
            }
        }
    }
}
