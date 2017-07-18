using System;
using System.Diagnostics;

namespace EMS.Core.Interfaces
{
    public interface IProcessAPI
    {
        event EventHandler<Process> ForegroundProcessChanged;
        event EventHandler<Process[]> ActiveProcessesChanged;

        Process GetForegroundProcess();

        Process[] GetActiveProcesses();

        string GetForegroundProcessName();
    }
}
