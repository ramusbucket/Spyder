using System.Threading.Tasks;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;

namespace EMS.Desktop.Headless
{
    public class DisplayListener : BaseListener<CapturedDisplaySnapshotDTO, byte[]>
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

        public async override Task Start()
        {
            await base.Start();

            this.displayApi.OnDisplaySnapshotTaken += OnDisplaySnapshotTakenHandler;

            await Task.Run(
                () => this.displayApi.StartWatchingDisplay());
        }

        public override void Stop()
        {
            this.displayApi.StopWatchingDisplay();
        }

        private void OnDisplaySnapshotTakenHandler(object sender, byte[] e)
        {
            var capturedItem = new CapturedDisplaySnapshotDTO
            {
                DisplaySnapshot = e,
                CreatedOn = TimeProvider.Current.Now
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
