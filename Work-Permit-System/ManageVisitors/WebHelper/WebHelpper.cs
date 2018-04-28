using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WorkPermitSystem
{
    public class WebHelpper
    {
        public static string connectionstring = "";
        public async Task<string> MakeGetRequest(string url)
        {
            var request = HttpWebRequest.Create(url);

            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return response.StatusCode + "Response contained an empty body...";
                    }
                    else
                    {
                        return content;
                    }
                }
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }
        public async Task<string> MakePostRequest(string url, string serializedDataString, bool isJson = true)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (isJson)
                request.ContentType = "application/json";
            else
                request.ContentType = "application/x-www-form-urlencoded";

            request.Method = "POST";
            var stream = await request.GetRequestStreamAsync();
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(serializedDataString);
                writer.Flush();
                writer.Dispose();
            }

            var response = await request.GetResponseAsync();
            var respStream = response.GetResponseStream();

            using (StreamReader sr = new StreamReader(respStream))
            {
                return sr.ReadToEnd();
            }
        }

        public String SerializeContent(Object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

    }
}