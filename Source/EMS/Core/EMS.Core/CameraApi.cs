using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using AForge.Video.DirectShow;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using EMS.Infrastructure.Common.Configurations;
using EMS.Infrastructure.Common.Utils;

namespace EMS.Core
{
    public class CameraApi : ICameraApi
    {
        public event EventHandler<byte[]> OnWebcamSnapshotTaken;

        private const int DefaultSnapshotPeriod = 2000;
        private const int DefaultSnapshotDueTime = 1000;

        private bool shouldSaveSnapshot;
        private CameraApiConfig config;
        private Timer webcamSnapshotTimer;
        private ConcurrentDictionary<string, VideoCaptureDevice> cameras;

        public CameraApi(CameraApiConfig config)
        {
            this.config = config;
            this.cameras = new ConcurrentDictionary<string, VideoCaptureDevice>();
        }

        public IList<WebcamDetails> GetAvailableWebcams()
        {
            var availableWebcams = new List<WebcamDetails>();
            var webcamsFilterInfo = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo info in webcamsFilterInfo)
            {
                availableWebcams.Add(
                    new WebcamDetails
                    {
                        Id = info.MonikerString,
                        Name = info.Name
                    });
            }

            return availableWebcams;
        }

        public void StartCamera(string cameraId)
        {
            if (string.IsNullOrEmpty(cameraId))
            {
                throw new ArgumentNullException($"Parameter \"{nameof(cameraId)}\" must not be null or empty.");
            }

            var availableWebcams = this.GetAvailableWebcams();

            if (availableWebcams == null || availableWebcams.Count == 0)
            {
                throw new ArgumentNullException("There are no available webcams at this moment");
            }

            if (!availableWebcams.Any(x => x.Id == cameraId))
            {
                throw new InvalidOperationException($"Webcam with id \"{cameraId}\" is not available at this moment.");
            }

            if (this.webcamSnapshotTimer == null)
            {
                InitializeSnapshotTimer();
            }

            var webcam = new VideoCaptureDevice(cameraId);
            this.cameras.AddOrUpdate(cameraId, webcam, (id, oldWebcam) =>
            {
                if (oldWebcam != null && oldWebcam.IsRunning)
                {
                    oldWebcam.Stop();
                }

                return webcam;
            });

            webcam.NewFrame += (sender, args) =>
            {
                if (this.shouldSaveSnapshot)
                {
                    var snapshot = (Bitmap)args.Frame.Clone();
                    var snapshotAsByteArray = snapshot.ToByteArray();
                    this.OnWebcamSnapshotTaken.Invoke(this, snapshotAsByteArray);

                    this.shouldSaveSnapshot = false;
                }
            };

            webcam.Start();
        }

        public void StopCamera(string cameraId)
        {
            if (this.cameras != null)
            {
                var camera = this.cameras[cameraId];

                if (camera.IsRunning)
                {
                    camera.Stop();
                }
            }
        }

        private void InitializeSnapshotTimer()
        {
            var dueTime = this.config.SnapshotTimerConfig != null ?
                this.config.SnapshotTimerConfig.DueTime :
                DefaultSnapshotDueTime;

            var period = this.config.SnapshotTimerConfig != null ?
                this.config.SnapshotTimerConfig.DueTime :
                DefaultSnapshotPeriod;

            this.webcamSnapshotTimer = new Timer((_) =>
            {
                this.shouldSaveSnapshot = true;
            },
            null,
            dueTime,
            period);
        }
    }
}
