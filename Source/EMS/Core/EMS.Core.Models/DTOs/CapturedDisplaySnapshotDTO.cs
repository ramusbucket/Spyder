namespace EMS.Core.Models.DTOs
{
    public class CapturedDisplaySnapshotDto : AuditableDto
    {
        public byte[] DisplaySnapshot { get; set; }
    }
}
