using System;
using System.Threading.Tasks;
using Easy.Common.Interfaces;
using EMS.Core.Interfaces;
using EMS.Core.Models.DTOs;
using EMS.Desktop.Client.Attributes;
using EMS.Desktop.Client.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;

namespace EMS.Desktop.Client.Listeners
{
    [DisabledListener]
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

        public override async Task Start()
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
