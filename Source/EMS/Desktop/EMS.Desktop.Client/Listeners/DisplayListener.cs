using System.Threading.Tasks;
using Easy.Common.Interfaces;
using EMS.Core.Interfaces;
using EMS.Core.Models.DTOs;
using EMS.Desktop.Client.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;

namespace EMS.Desktop.Client.Listeners
{
    public class DisplayListener : BaseListener<CapturedDisplaySnapshotDto>
    {
        private IDisplayApi displayApi;

        public DisplayListener(
            IRestClient httpClient,
            ILogger logger,
            IDisplayApi displayApi,
            DisplayListenerConfig config)
            : base(httpClient, logger, config)
        {
            this.displayApi = displayApi;
        }

        public override async Task Start()
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
            var capturedItem = new CapturedDisplaySnapshotDto
            {
                DisplaySnapshot = e,
                CreatedOn = TimeProvider.Current.Now,
                SessionId = Identity.SessionId
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
