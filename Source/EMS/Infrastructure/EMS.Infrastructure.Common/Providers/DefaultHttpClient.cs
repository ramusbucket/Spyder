using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace EMS.Infrastructure.Common.Providers
{
    public class DefaultHttpClient : IHttpClient
    {
        private HttpClient httpClient;

        public DefaultHttpClient(string baseAddress)
        {
            this.httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
        }

        public DefaultHttpClient()
        {
            this.httpClient = new HttpClient();
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return this.httpClient.GetAsync(requestUri);
        }

        public Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T dataToSend)
            where T : class
        {
            return this.httpClient.PostAsJsonAsync(requestUri, dataToSend);
        }

        public Task<HttpResponseMessage> PostAsync<T>(string requestUri, T dataToSend, MediaTypeFormatter mediaFormatter)
            where T : class
        {
            return this.httpClient.PostAsync(requestUri, dataToSend, mediaFormatter);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return this.httpClient.SendAsync(request);
        }
    }
}
