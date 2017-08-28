using System.Collections.Generic;

namespace EMS.Core.Models.DTOs
{
    public class CapturedActiveProcessesDto : AuditableDto
    {
        public IEnumerable<SlimProcess> CapturedActiveProcesses { get; set; }
    }
}
