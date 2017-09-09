﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Easy.Common.Interfaces;
using EMS.Core.Interfaces;
using EMS.Core.Models.DTOs;
using EMS.Desktop.Client.Attributes;
using EMS.Desktop.Client.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Serilog;

namespace EMS.Desktop.Client.Listeners
{
    [DisabledListener]
    public class ActiveProcessesListener : BaseListener<CapturedActiveProcessesDto>
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

        public override async Task Start()
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
            var capturedItem = new CapturedActiveProcessesDto
            {
                CapturedActiveProcesses = e.Select(x => x.ProjectToSlimProcess()),
                CreatedOn = TimeProvider.Current.Now,
                SessionId = Identity.SessionId
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
