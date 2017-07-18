using System;
using System.Diagnostics;
using System.Threading;
using EMS.Core.Interfaces;
using EMS.Core.Interfaces.Providers;
using EMS.Infrastructure.Common.Configurations;

namespace EMS.Core
{
    public class ProcessApi : IProcessApi
    {
        private Process lastForegroundProcess;
        private Process[] lastActiveProcesses;
        private Timer getForegroundWindowTimer;
        private Timer getProcessStartedTimer;
        private IWin32ApiProvider win32ApiProvider;
        private ProcessAPIConfig config;

        public event EventHandler<Process> OnForegroundProcessChanged;
        public event EventHandler<Process[]> OnActiveProcessesChanged;

        public ProcessApi(IWin32ApiProvider win32ApiProvider, ProcessAPIConfig config)
        {
            this.win32ApiProvider = win32ApiProvider;
            this.config = config;
        }

        public Process GetForegroundProcess()
        {
            var foregroundWindowHandle = this.win32ApiProvider.GetForegroundWindowManaged();

            // This check must be done because
            // the foreground window can be NULL in certain cases, 
            // such as when a window is losing activation.
            if (foregroundWindowHandle == null)
            {
                return null;
            }

            var processId = default(uint);
            this.win32ApiProvider.GetWindowThreadProcessIdManaged(foregroundWindowHandle, out processId);

            return Process.GetProcessById((int)processId);
        }

        public string GetForegroundProcessName()
        {
            var process = GetForegroundProcess();
            var processName =
                    process != null ?
                    process.ProcessName :
                    string.Empty;

            return processName;
        }

        public Process[] GetActiveProcesses()
        {
            return Process.GetProcesses();
        }

        public void StartTimerForForegroundProcessChanged()
        {
            this.lastForegroundProcess = GetForegroundProcess();
            this.getForegroundWindowTimer = CreateForegroundProcessTimer();
        }

        public void StartTimerForActiveProcessesChanged()
        {
            this.lastActiveProcesses = GetActiveProcesses();
            this.getProcessStartedTimer = CreateGetProcessesListTimer();
        }

        private Timer CreateForegroundProcessTimer()
        {
            return new Timer(
                callback: (x) =>
                {
                    var currentForegroundProcess = GetForegroundProcess();

                    if (currentForegroundProcess != null &&
                        this.lastForegroundProcess != null &&
                        this.lastForegroundProcess.Id != currentForegroundProcess.Id)
                    {
                        this.OnForegroundProcessChanged(this, currentForegroundProcess);
                        this.lastForegroundProcess = currentForegroundProcess;
                    }
                },
                state: null,
                dueTime: this.config.ForegroundProcessChangedTimerConfig != null ? this.config.ForegroundProcessChangedTimerConfig.DueTime : 100,
                period: this.config.ForegroundProcessChangedTimerConfig != null ? this.config.ForegroundProcessChangedTimerConfig.Period : 100);
        }

        private Timer CreateGetProcessesListTimer()
        {
            return new Timer(
                callback: (x) =>
                {
                    var currentActiveProcesses = GetActiveProcesses();

                    if (currentActiveProcesses != null &&
                        this.lastActiveProcesses != null &&
                        this.lastActiveProcesses.Length != currentActiveProcesses.Length)
                    {
                        this.OnActiveProcessesChanged(this, currentActiveProcesses);
                        this.lastActiveProcesses = currentActiveProcesses;
                    }
                },
                state: null,
                dueTime: this.config.ActiveProcessesChangedTimerConfig != null ? this.config.ActiveProcessesChangedTimerConfig.DueTime : 100,
                period: this.config.ActiveProcessesChangedTimerConfig != null ? this.config.ActiveProcessesChangedTimerConfig.Period : 100);
        }
    }
}
