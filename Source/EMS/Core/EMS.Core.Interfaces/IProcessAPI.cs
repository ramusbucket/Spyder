using System;
using System.Diagnostics;

namespace EMS.Core.Interfaces
{
    public interface IProcessApi
    {
        event EventHandler<Process> OnForegroundProcessChanged;
        event EventHandler<Process[]> OnActiveProcessesChanged;

        Process GetForegroundProcess();

        Process[] GetActiveProcesses();

        string GetForegroundProcessName();
    }
}
