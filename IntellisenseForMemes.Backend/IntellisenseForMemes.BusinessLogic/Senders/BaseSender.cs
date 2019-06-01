using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IntellisenseForMemes.BusinessLogic.Senders
{
    public class BaseSender : IBaseSender
    {

        public BaseSender()
        {
        }

        public async Task<string> SendRequest(string url, object body, HttpMethod method, Dictionary<string, string> headers, bool ignoreNullProperties = false)
        {
            var message = new HttpRequestMessage(method, new Uri(url));

            var json = body != null ? JsonConvert.SerializeObject(body) : null;
            if (ignoreNullProperties) {
                json = body != null 
                    ? JsonConvert.SerializeObject(body, Formatting.None, new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore
                    })
                    : null;
            }

            if (!string.IsNullOrEmpty(json))
            {
                message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "IntellisenseForMemes");

                if (headers != null && headers.Any())
                {
                    foreach (var header in headers)
                    {
                        try
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                        catch (FormatException)
                        {
                            var headerIsAdded = client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                            if (!headerIsAdded)
                            {
                                throw new ArgumentException($"Header {header.Key} is invalid");
                            }
                        }
                    }
                }

                client.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.SendAsync(message);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return responseBody;
                }
                
                //_logger.Log(LogLevel.Error, $"Request to {url} failed with code {response.StatusCode} and message \"{responseBody}\" \n\n\n\n\n" +
                //              $"Response body \"{await response.Content.ReadAsStringAsync()}\"");

                throw new HttpRequestException(responseBody);
            }
        }
    }
}
