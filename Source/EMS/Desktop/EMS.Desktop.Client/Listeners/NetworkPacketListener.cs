using Easy.Common.Interfaces;
using EMS.Core.Interfaces;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;
using System;
using System.Threading.Tasks;
using EMS.Core.Models.DTOs;
using EMS.Desktop.Client.Models;

namespace EMS.Desktop.Client
{
    public class NetworkPacketListener : BaseListener<CapturedNetworkPacketDetailsDto>
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
            var capturedItem = new CapturedNetworkPacketDetailsDto
            {
                NetworkPacket = e,
                CreatedOn = TimeProvider.Current.Now,
                SessionId = Identity.SessionId
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
