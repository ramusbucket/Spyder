namespace EMS.Core.Models
{
    public class CapturedNetworkPacketDetails : Auditable
    {
        public byte[] NetworkPacket { get; set; }
    }
}
