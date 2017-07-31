using System.Linq;
using System.Threading.Tasks;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;

namespace EMS.Desktop.Headless
{
    public class CameraListener : BaseListener<CapturedCameraSnapshotDetails, byte[]>
    {
        private ICameraApi cameraApi;
        private string cameraId;

        public CameraListener(
            IHttpClient httpClient,
            ILogger logger,
            ICameraApi cameraApi,
            CameraListenerConfig config)
            : base(httpClient, logger, config)
        {
            this.cameraApi = cameraApi;
            this.cameraId = this.cameraApi.GetAvailableWebcams()?.FirstOrDefault()?.Id;
        }

        public async override Task Start()
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
            var capturedItem = new CapturedCameraSnapshotDetails
            {
                CameraSnapshot = e,
                CreatedOn = TimeProvider.Current.Now
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
