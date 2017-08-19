﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;
using System.Linq;
using Easy.Common.Interfaces;

namespace EMS.Desktop.Client
{
    public class ActiveProcessesListener : BaseListener<CapturedActiveProcessesDTO, Process[]>
    {
        private IProcessApi processApi;

        public ActiveProcessesListener(
            IRestClient httpClient,
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
            var capturedItem = new CapturedActiveProcessesDTO
            {
                CapturedActiveProcesses = e.Select(x => x.ProjectToSlimProcess()),
                CreatedOn = TimeProvider.Current.Now
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
