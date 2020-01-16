using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace App1.Services
{
    public class ApiService
    {
        private readonly string BaseAddress;
        public static ApiService Instance;

        static ApiService()
        {
            Instance = new ApiService();
        }

        private ApiService() 
        {
            BaseAddress = "http://192.168.10.132:80/Service/Service.svc";
        }
            

        public bool Login (string id, string pw)
        {
            //
            // Send data
            //
            JObject json = new JObject();
            json.Add("id", id);
            json.Add("pw", pw);
            UTF8Encoding encoder = new UTF8Encoding();
            byte[] data = encoder.GetBytes(json.ToString());

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(BaseAddress + "/login");
            req.Method = "POST";
            req.ContentType = "text/json";
            req.GetRequestStream().Write(data, 0, data.Length);
            //
            // Recevie data
            //
            string responseText = default;
            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }catch (WebException we)
            {
                Console.WriteLine($"Login occur exception: {we.StackTrace}");
                return false;
            }
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Login HttpStatusCode:{resp.StatusCode}");
                return false;
            }
            StreamReader sr = new StreamReader(resp.GetResponseStream());
            responseText = sr.ReadToEnd();
            if (responseText == default)
            {
                Console.WriteLine($"Login No ResponseText");
                return false;
            }
            //
            // Convert
            //
            ReturnCode ret = JsonConvert.DeserializeObject<ReturnCode>(responseText);
            if (ret == default)
            {
                Console.WriteLine($"Login Fail jsonconverting");
                return false;
            }
            int userIdx = -1;
            int.TryParse(ret.value, out userIdx);
            if (userIdx == -1)
            {
                Console.WriteLine($"Login No User");
                return false;
            }
            return true;
        }

    }
}
