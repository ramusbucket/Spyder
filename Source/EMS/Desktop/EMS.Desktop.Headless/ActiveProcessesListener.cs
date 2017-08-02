using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;
using System.Linq;

namespace EMS.Desktop.Headless
{
    public class ActiveProcessesListener : BaseListener<CapturedActiveProcessesDetails, Process[]>
    {
        private IProcessApi processApi;

        public ActiveProcessesListener(
            IHttpClient httpClient,
            ILogger logger,
            IProcessApi processApi,
            ActiveProcessesListenerConfig config)
            : base(httpClient, logger, config)
        {
            this.processApi = processApi;
        }

        public async override Task Start()
        {
            await base.Start();

            this.processApi.OnActiveProcessesChanged += OnActiveProcessesChangedHandler;

            await Task.Run(
                () => this.processApi.StartListeningForActiveProcessesChanged());
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        private void OnActiveProcessesChangedHandler(object sender, Process[] e)
        {
            var capturedItem = new CapturedActiveProcessesDetails
            {
                CapturedActiveProcesses = e.Select(x => x.ProjectToSlimProcess()),
                CreatedOn = TimeProvider.Current.Now
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
