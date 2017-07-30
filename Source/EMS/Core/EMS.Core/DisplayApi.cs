using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using EMS.Core.Interfaces;
using EMS.Infrastructure.Common.Configurations;
using EMS.Infrastructure.Common.Utils;

namespace EMS.Core
{
    public class DisplayApi : IDisplayApi
    {
        private DisplayApiConfig config;
        private bool isWatchingDisplay;

        public event EventHandler<byte[]> OnDisplaySnapshotTaken;

        public DisplayApi(DisplayApiConfig config)
        {
            this.config = config;
        }

        public void StartWatchingDisplay()
        {
            this.isWatchingDisplay = true;
            var sleepInterval = this.config != null ?
                this.config.DisplayWatcherSleepIntervalInMilliseconds :
                5000;

            var primaryScreenBounds = Screen.PrimaryScreen.Bounds;
            var primaryScreenWidth = primaryScreenBounds.Width;
            var primaryScreenHeight = primaryScreenBounds.Height;

            using (var bitmapScreenCapture = new Bitmap(primaryScreenWidth, primaryScreenHeight))
            {
                using (var graphics = Graphics.FromImage(bitmapScreenCapture))
                {
                    while (this.isWatchingDisplay)
                    {
                        graphics.CopyFromScreen(
                            primaryScreenBounds.X,
                            primaryScreenBounds.Y,
                            0, 0,
                            bitmapScreenCapture.Size,
                            CopyPixelOperation.SourceCopy);

                        var imageAsByteArray = Converter.ToByteArray(bitmapScreenCapture);

                        this.OnDisplaySnapshotTaken.Invoke(this, imageAsByteArray);

                        Thread.Sleep(sleepInterval);
                    }
                }
            }
        }

        public void StopWatchingDisplay()
        {
            this.isWatchingDisplay = false;
        }
    }
}
