using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EMS.Core.Models
{
    public class CapturedActiveProcessesDetails : Auditable
    {
        public IEnumerable<SlimProcess> CapturedActiveProcesses { get; set; }
    }

    public static class ProcessExtensions
    {
        public static SlimProcess ProjectToSlimProcess(this Process process)
        {
            return new SlimProcess
            {
                ProcessId = process.Id,
                ProcessName = process.ProcessName,
                MachineName = process.MachineName,
                MainWindowTitle = process.MainWindowTitle,
                ProcessStartInfo = process.StartInfo.ProjectToSlimProcessStartInfo(),
                PhisycalMemoryAllocated = process.WorkingSet64,
                VirtualMemoryAllocated = process.VirtualMemorySize64,
            };
        }

        public static SlimProcessStartInfo ProjectToSlimProcessStartInfo(this ProcessStartInfo processStartInfo)
        {
            return new SlimProcessStartInfo
            {
                Arguments = processStartInfo.Arguments,
                CreateNoWindow = processStartInfo.CreateNoWindow,
                Domain = processStartInfo.Domain,
                FileName = processStartInfo.FileName,
                UserName = processStartInfo.UserName,
                WorkingDirectory = processStartInfo.WorkingDirectory,
                UseShellExecute = processStartInfo.UseShellExecute
            };
        }
    }

    public class SlimProcess
    {
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string MachineName { get; set; }
        public string MainWindowTitle { get; set; }
        public TimeSpan UserProcessorTime { get; set; }
        public TimeSpan TotalProcessorTime { get; set; }
        public long VirtualMemoryAllocated { get; set; }
        public long PhisycalMemoryAllocated { get; set; }
        public SlimProcessStartInfo ProcessStartInfo { get; set; }
    }

    public class SlimProcessStartInfo
    {
        public string Domain { get; set; }
        public string FileName { get; set; }
        public string UserName { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public bool CreateNoWindow { get; set; }
        public bool UseShellExecute { get; set; }
    }
}
