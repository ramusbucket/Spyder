namespace EMS.Core.Models.DTOs
{
    public class CapturedActiveProcessesDto : AuditableDto
    {
        public SlimProcess CapturedActiveProcesses { get; set; }
    }
}
