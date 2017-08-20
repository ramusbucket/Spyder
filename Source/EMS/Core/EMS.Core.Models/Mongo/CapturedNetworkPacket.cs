namespace EMS.Web.Common.Mongo
{
    public class CapturedNetworkPacket : Auditable
    {
        public string Protocol { get; set; }
        public ushort HostPort { get; set; }
        public string HostAddress { get; set; }
        public ushort DestinationPort { get; set; }
        public string DestinationAddress { get; set; }
    }
}
