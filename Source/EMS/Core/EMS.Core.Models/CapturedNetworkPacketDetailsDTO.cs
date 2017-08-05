namespace EMS.Core.Models
{
    public class CapturedNetworkPacketDetailsDTO : Auditable
    {
        public byte[] NetworkPacket { get; set; }
    }
}
