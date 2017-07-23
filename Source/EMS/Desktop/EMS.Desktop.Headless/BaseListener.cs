using System.Threading.Tasks;
using EMS.Infrastructure.Common.Providers;

namespace EMS.Desktop.Headless
{
    public abstract class BaseListener : IListener
    {
        protected IHttpClient httpClient;

        public BaseListener(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public abstract Task Start();

        public abstract void Stop();
    }
}
