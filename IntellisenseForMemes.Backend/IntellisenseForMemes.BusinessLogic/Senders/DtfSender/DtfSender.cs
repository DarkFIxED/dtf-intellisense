using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IntellisenseForMemes.BusinessLogic.Senders.DtfSender.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IntellisenseForMemes.BusinessLogic.Senders.DtfSender
{
    public class DtfSender : BaseSender, IDtfSender
    {
        private readonly string _baseUrl = "https://api.dtf.ru/v1.6";

        //TODO: Move this in settings
        private readonly string _accessToken = "42a241a144abbfb4f189e53d5f63ecabe759a6cff349ee8f3c96ec27096ce4cb";

        private readonly Dictionary<string, string> _baseHeaders;

        private readonly ILogger _logger;

        public DtfSender(ILogger logger)
        {
            _logger = logger;
            _baseHeaders = new Dictionary<string, string> { { "X-Device-Token", _accessToken } };
        }

        public async Task<string> GetArticles(int count, int offset)
        {
            var url = _baseUrl + $"/timeline/gamedev/popular?count={count}&offset={offset}";

            var responseJson = await SendRequest(url, null, HttpMethod.Get, _baseHeaders);

            return responseJson;
        }

        public async Task<string> UploadAttachment(string attachmentUrl)
        {
            var url = _baseUrl + "/uploader/extract";

            var requestParameters = new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string,string>("url", attachmentUrl)
                };

            var responseJson = await SendDtfRequest(url, requestParameters);
            var response = JsonConvert.DeserializeObject<DtfResponseModel<object>>(responseJson);

            return response.Result.First().ToString();
        }

        public async Task<string> PostComment(int articleId, int replyTo, string text, string dtfAttachmentObject)
        {
            var url = _baseUrl + "/comment/add";

            var requestParameters = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string,string>("id", articleId.ToString()),
                new KeyValuePair<string,string>("reply_to", replyTo.ToString()),
                new KeyValuePair<string,string>("text", text),
                new KeyValuePair<string,string>("attachments", "[" + dtfAttachmentObject + "]")
            };

            var responseJson = await SendDtfRequest(url, requestParameters);

            return responseJson;
        }

        public async Task<string> SendDtfRequest(string url, List<KeyValuePair<string, string>> requestParameters)
        {
            var content = new FormUrlEncodedContent(requestParameters);
            using (var client = new HttpClient())
            {
                foreach (var (key, value) in _baseHeaders)
                {
                    client.DefaultRequestHeaders.Add(key, value);
                }
                client.DefaultRequestHeaders.Add("User-Agent", "IntellisenseForMemes");

                var response = await client.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return responseBody;
                }

                throw new HttpRequestException(responseBody);
            }
        }

    }
}
