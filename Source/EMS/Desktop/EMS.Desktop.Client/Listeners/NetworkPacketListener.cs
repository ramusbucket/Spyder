using Easy.Common.Interfaces;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;
using System;
using System.Threading.Tasks;

namespace EMS.Desktop.Client
{
    public class NetworkPacketListener : BaseListener<CapturedNetworkPacketDetailsDTO, byte[]>
    {
        private INetworkApi networkApi;
        private string cameraId;

        public NetworkPacketListener(
            IRestClient httpClient,
            ILogger logger,
            INetworkApi networkApi,
            NetworkListenerConfig config)
            : base(httpClient, logger, config)
        {
            this.networkApi = networkApi;
        }

        public async override Task Start()
        {
            await base.Start();

            this.networkApi.OnPacketSniffed += OnPacketSniffedHandler;

            await Task.Run(
                () => this.networkApi.StartSniffingAllAddresses());
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        private void OnPacketSniffedHandler(object sender, byte[] e)
        {
            var capturedItem = new CapturedNetworkPacketDetailsDTO
            {
                NetworkPacket = e,
                CreatedOn = TimeProvider.Current.Now
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
