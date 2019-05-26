using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IntellisenseForMemes.BusinessLogic.Senders
{
    public interface IBaseSender
    {
        Task<string> SendRequest(string url, object body, HttpMethod method, Dictionary<string, string> headers, bool ignoreNullProperties = false);
    }
}