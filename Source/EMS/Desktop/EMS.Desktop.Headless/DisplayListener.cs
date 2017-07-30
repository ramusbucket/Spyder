using System.Threading.Tasks;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;

namespace EMS.Desktop.Headless
{
    public class DisplayListener : BaseListener<CapturedDisplaySnapshotDetails, byte[]>
    {
        private IDisplayApi displayApi;

        public DisplayListener(
            IHttpClient httpClient,
            ILogger logger,
            IDisplayApi displayApi,
            DisplayListenerConfig config)
            : base(httpClient, logger, config)
        {
            this.displayApi = displayApi;
        }

        public override Task Start()
        {
            base.Start().Wait();

            this.displayApi.OnDisplaySnapshotTaken += OnDisplaySnapshotTakenHandler;
            return Task.Run(() => this.displayApi.StartWatchingDisplay());
        }

        public override void Stop()
        {
            this.displayApi.StopWatchingDisplay();
        }

        private void OnDisplaySnapshotTakenHandler(object sender, byte[] e)
        {
            var capturedSnapshot = new CapturedDisplaySnapshotDetails
            {
                DisplaySnapshot = e,
                CreatedOn = TimeProvider.Current.Now
            };

            this.capturedItems.Enqueue(capturedSnapshot);
        }
    }
}
