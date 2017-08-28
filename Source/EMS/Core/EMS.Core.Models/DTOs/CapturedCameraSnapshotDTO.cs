namespace EMS.Core.Models.DTOs
{
    public class CapturedCameraSnapshotDto : AuditableDto
    {
        public byte[] CameraSnapshot { get; set; }
    }
}
