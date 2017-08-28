using System;
using System.Diagnostics;

namespace EMS.Core.Models.DTOs
{
    public static class ProcessExtensions
    {
        public static SlimProcess ProjectToSlimProcess(this Process process)
        {
            var processDetails = new SlimProcess
            {
                ProcessId = process.Id,
                ProcessName = process.ProcessName,
                MachineName = process.MachineName,
                MainWindowTitle = process.MainWindowTitle,
                PhisycalMemoryAllocated = process.WorkingSet64,
                VirtualMemoryAllocated = process.VirtualMemorySize64,
            };

            try
            {
                processDetails.StartTime = process.StartTime;
                processDetails.TotalProcessorTime = process.TotalProcessorTime;
                processDetails.UserProcessorTime = process.UserProcessorTime;
            }
            catch (Exception)
            {
                // Exception might be thrown here for a small subset of system processes 
                // which are not of interest for our application
                // that is why we can safely ignore it
            }

            return processDetails;
        }
    }
}