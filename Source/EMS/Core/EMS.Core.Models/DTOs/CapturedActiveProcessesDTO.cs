using EMS.Core.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EMS.Core.Models
{
    public class CapturedActiveProcessesDTO : BaseDTO
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
}
