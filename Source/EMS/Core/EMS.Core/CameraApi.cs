using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Core.Interfaces;
using AForge.Video.DirectShow;
using AForge.Video;
using System.Threading;
using System.Drawing;
using EMS.Infrastructure.Common.Utils;

namespace EMS.Core
{
    public class CameraApi : ICameraApi
    {
        public event EventHandler<byte[]> OnWebcamSnapshotTaken;

        private Timer webcamSnapshotTimer;
        private FilterInfoCollection availableWebcams;
        private VideoCaptureDevice camera;
        private bool shouldSaveSnapshot;

        public CameraApi()
        {
            availableWebcams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        public bool IsWebcamAvailable()
        {
            return availableWebcams.Count > 0;
        }

        public void StartCamera(uint index = 0)
        {
            if (availableWebcams == null || availableWebcams.Count < 1)
            {
                throw new InvalidOperationException("No webcams are available at this moment.");
            }

            var lastAvailableCameraIndex = availableWebcams.Count - 1;
            if (index > lastAvailableCameraIndex)
            {
                throw new ArgumentOutOfRangeException($"Webcam index should be in the range between {0} and {lastAvailableCameraIndex} inclusive.");
            }

            this.webcamSnapshotTimer = new Timer((_) =>
            {
                this.shouldSaveSnapshot = true;
            },
            null,
            1000,
            2000);

            camera = new VideoCaptureDevice(this.availableWebcams[(int)index].MonikerString);
            camera.NewFrame += (sender, args) =>
             {
                 if (this.shouldSaveSnapshot)
                 {
                     var snapshot = (Bitmap)args.Frame.Clone();
                     var snapshotAsByteArray = snapshot.ToByteArray();
                     this.OnWebcamSnapshotTaken.Invoke(this, snapshotAsByteArray);

                     this.shouldSaveSnapshot = false;
                 }
             };

            camera.Start();
        }

        public void StopCamera()
        {
            if (this.camera.IsRunning)
            {
                this.camera.Stop();
            }
        }
    }
}
