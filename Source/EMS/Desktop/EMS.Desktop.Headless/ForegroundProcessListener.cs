using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;

namespace EMS.Desktop.Headless
{
    public class ForegroundProcessListener : BaseListener<CapturedForegroundProcessDetails, Process>
    {
        private IProcessApi processApi;

        public ForegroundProcessListener(
            IHttpClient httpClient,
            ILogger logger,
            IProcessApi processApi,
            ForegroundProcessListenerConfig config)
            : base(httpClient, logger, config)
        {
            this.processApi = processApi;
        }

        public async override Task Start()
        {
            await base.Start();

            this.processApi.OnForegroundProcessChanged += OnForegroundProcessChangedHandler;

            await Task.Run(
                () => this.processApi.StartListeningForForegroundProcessChanged());
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        private void OnForegroundProcessChangedHandler(object sender, Process e)
        {
            var capturedItem = new CapturedForegroundProcessDetails
            {
                CapturedForegroundProcess = e.ProjectToSlimProcess(),
                CreatedOn = TimeProvider.Current.Now
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
