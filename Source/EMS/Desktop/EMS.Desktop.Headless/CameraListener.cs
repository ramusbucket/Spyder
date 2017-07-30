using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Core.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;

namespace EMS.Desktop.Headless
{
    public class CameraListener : BaseListener<CapturedCameraImageDetails, byte[]>
    {
        public CameraListener(
            IHttpClient httpClient, 
            ILogger logger,
            CameraListenerConfig config) 
            : base(httpClient, logger, config)
        {
        }

        public override Task Start()
        {
            return base.Start();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
