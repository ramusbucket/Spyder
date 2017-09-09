using System;
using System.Diagnostics;
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
    public class ForegroundProcessListener : BaseListener<CapturedForegroundProcessDto>
    {
        private IProcessApi processApi;

        public ForegroundProcessListener(
            IRestClient httpClient,
            ILogger logger,
            IProcessApi processApi,
            ForegroundProcessListenerConfig config)
            : base(httpClient, logger, config)
        {
            this.processApi = processApi;
        }

        public override async Task Start()
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
            var capturedItem = new CapturedForegroundProcessDto
            {
                CapturedForegroundProcess = e.ProjectToSlimProcess(),
                CreatedOn = TimeProvider.Current.Now,
                SessionId = Identity.SessionId
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
