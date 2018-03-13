using IntelligentHack.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IntelligentHack.Bot.Helpers
{
    public class RestHelper
    {
        public static async Task<List<Person>> ImageVerification(string fileName)
        {
            using (var client = new HttpClient())
            {
                var service = $"{Settings.FunctionURL}/api/ImageVerification/";

                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                var token = Convert.ToBase64String(time.Concat(key).ToArray());
                token = SecurityHelper.Encrypt(token, Settings.Cryptography);

                ImageVerificationRequest request = new ImageVerificationRequest();
                request.Token = token;
                request.ImageName = fileName;

                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var httpResponse = client.PostAsync(service, content).Result;

                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var str = await httpResponse.Content.ReadAsStringAsync();
                        List<Person> result = JsonConvert.DeserializeObject<List<Person>>(str);
                        return result;
                    }
                }
            }
            return null;
        }
    }
}