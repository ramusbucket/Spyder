using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using EMS.Core.Interfaces;
using EMS.Core.Interfaces.Providers;
using EMS.Infrastructure.Common.Configurations;

namespace EMS.Core
{
    public class ProcessApi : IProcessApi
    {
        private Process _lastForegroundProcess;
        private HashSet<string> _activeProcessesSet;
        private Timer _getForegroundWindowTimer;
        private Timer _getProcessStartedTimer;
        private readonly IWin32ApiProvider _win32ApiProvider;
        private readonly ProcessApiConfig _config;

        public event EventHandler<Process> OnForegroundProcessChanged;
        public event EventHandler<Process[]> OnActiveProcessesChanged;

        public ProcessApi(IWin32ApiProvider win32ApiProvider, ProcessApiConfig config)
        {
            this._win32ApiProvider = win32ApiProvider;
            this._config = config;
        }

        public Process GetForegroundProcess()
        {
            var foregroundWindowHandle = this._win32ApiProvider.GetForegroundWindowManaged();

            // This check must be done because
            // the foreground window can be NULL in certain cases, 
            // such as when a window is losing activation.
            if (foregroundWindowHandle == null)
            {
                return null;
            }

            var processId = default(uint);
            this._win32ApiProvider.GetWindowThreadProcessIdManaged(foregroundWindowHandle, out processId);

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

        public void StartListeningForForegroundProcessChanged()
        {
            this._lastForegroundProcess = GetForegroundProcess();
            this._getForegroundWindowTimer = CreateForegroundProcessTimer();
        }

        public void StartListeningForActiveProcessesChanged()
        {
            this._activeProcessesSet = GetActiveProcessesNames();
            this._getProcessStartedTimer = CreateGetProcessesListTimer();
        }

        private HashSet<string> GetActiveProcessesNames()
        {
            var activeProcesses = GetActiveProcesses();
            this.OnActiveProcessesChanged?.Invoke(this, activeProcesses);

            var collection = activeProcesses?
                .Select(x => x.ProcessName)
                .Distinct();

            return new HashSet<string>(collection ?? new List<string>());
        }

        private Timer CreateForegroundProcessTimer()
        {
            return new Timer(
                callback: (x) =>
                {
                    var currentForegroundProcess = GetForegroundProcess();

                    if (currentForegroundProcess != null &&
                        this._lastForegroundProcess != null &&
                        this._lastForegroundProcess.Id != currentForegroundProcess.Id)
                    {
                        this.OnForegroundProcessChanged(this, currentForegroundProcess);
                        this._lastForegroundProcess = currentForegroundProcess;
                    }
                },
                state: null,
                dueTime: this._config.ForegroundProcessChangedTimerConfig != null ? this._config.ForegroundProcessChangedTimerConfig.DueTime : 100,
                period: this._config.ForegroundProcessChangedTimerConfig != null ? this._config.ForegroundProcessChangedTimerConfig.Period : 100);
        }

        private Timer CreateGetProcessesListTimer()
        {
            return new Timer(
                callback: (x) =>
                {
                    var currentActiveProcesses = GetActiveProcesses();

                    if (currentActiveProcesses != null && _activeProcessesSet != null)
                    {
                        var newlyStartedProcesses = new List<Process>();
                        foreach (var process in currentActiveProcesses)
                        {
                            if (!_activeProcessesSet.Contains(process.ProcessName))
                            {
                                newlyStartedProcesses.Add(process);
                                _activeProcessesSet.Add(process.ProcessName);
                            }
                        }

                        this.OnActiveProcessesChanged?.Invoke(this, newlyStartedProcesses.ToArray());
                    }
                },
                state: null,
                dueTime: _config.ActiveProcessesChangedTimerConfig?.DueTime ?? 1000,
                period: _config.ActiveProcessesChangedTimerConfig?.Period ?? 1000);
        }
    }

    public class ProcessesComparer : IEqualityComparer<Process>
    {
        public bool Equals(Process x, Process y)
        {
            return x?.Id == y?.Id;
        }

        public int GetHashCode(Process obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
