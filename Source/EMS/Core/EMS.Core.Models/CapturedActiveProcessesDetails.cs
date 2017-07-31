using System.Diagnostics;

namespace EMS.Core.Models
{
    public class CapturedActiveProcessesDetails : Auditable
    {
        public Process[] CapturedActiveProcesses { get; set; }
    }
}
