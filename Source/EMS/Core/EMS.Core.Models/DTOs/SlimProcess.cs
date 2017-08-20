using System;

namespace EMS.Core.Models.DTOs
{
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
}
