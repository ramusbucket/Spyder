using System.Linq;
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
    public class CameraListener : BaseListener<CapturedCameraSnapshotDto>
    {
        private ICameraApi cameraApi;
        private string cameraId;

        public CameraListener(
            IRestClient httpClient,
            ILogger logger,
            ICameraApi cameraApi,
            CameraListenerConfig config)
            : base(httpClient, logger, config)
        {
            this.cameraApi = cameraApi;
            this.cameraId = this.cameraApi.GetAvailableWebcams()?.FirstOrDefault()?.Id;
        }

        public override async Task Start()
        {
            await base.Start();

            this.cameraApi.OnWebcamSnapshotTaken += OnWebcamSnapshotTakenHandler;

            await Task.Run(
                () => this.cameraApi.StartCamera(this.cameraId));
        }

        public override void Stop()
        {
            this.cameraApi.StopCamera(this.cameraId);
        }

        private void OnWebcamSnapshotTakenHandler(object sender, byte[] e)
        {
            var capturedItem = new CapturedCameraSnapshotDto
            {
                CameraSnapshot = e,
                CreatedOn = TimeProvider.Current.Now,
                SessionId = Identity.SessionId
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
