using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using EMS.Core.Interfaces;
using EMS.Core.Interfaces.Providers;

namespace EMS.Core
{
    public class ProcessAPI : IProcessAPI
    {
        private Process lastForegroundProcess;
        private Process[] lastActiveProcesses;
        private Timer getForegroundWindowTimer;
        private Timer getProcessStartedTimer;
        private IWin32ApiProvider win32ApiProvider;

        public event EventHandler<Process> ForegroundProcessChanged;
        public event EventHandler<Process[]> ActiveProcessesChanged;

        public ProcessAPI(IWin32ApiProvider win32ApiProvider)
        {
            this.win32ApiProvider = win32ApiProvider;

            this.lastForegroundProcess = GetForegroundProcess();
            this.lastActiveProcesses = GetActiveProcesses();

            this.getForegroundWindowTimer = CreateForegroundProcessTimer();
            this.getProcessStartedTimer = CreateGetProcessesListTimer();
        }

        /// <summary>
        /// Returns the current foreground process
        /// (the window on focus).
        /// </summary>
        /// <returns>
        /// The current foreground process or (in rare cases) null, 
        /// if the window is losing activation.
        /// </returns>
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

        /// <summary>
        /// Returns the name of the process owning the foreground window.
        /// </summary>
        /// <returns>
        /// The name of the currently active process or empty string 
        /// if there is no active window at this exact moment.
        /// </returns>
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
                        this.ForegroundProcessChanged(this, currentForegroundProcess);
                        this.lastForegroundProcess = currentForegroundProcess;
                    }
                },
                state: null,
                dueTime: 100,
                period: 100);
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
                        this.ActiveProcessesChanged(this, currentActiveProcesses);
                        this.lastActiveProcesses = currentActiveProcesses;
                    }
                },
                state: null,
                dueTime: 100,
                period: 100);
        }
    }
}
