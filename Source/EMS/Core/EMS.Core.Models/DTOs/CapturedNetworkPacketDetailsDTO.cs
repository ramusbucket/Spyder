namespace EMS.Core.Models.DTOs
{
    public class CapturedNetworkPacketDetailsDto : AuditableDto
    {
        public byte[] NetworkPacket { get; set; }
    }
}
