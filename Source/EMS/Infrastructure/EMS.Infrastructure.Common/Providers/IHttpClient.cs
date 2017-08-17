using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace EMS.Infrastructure.Common.Providers
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);

        Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T dataToSend)
            where T : class;

        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T dataToSend, MediaTypeFormatter mediaFormatter)
            where T : class;

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
