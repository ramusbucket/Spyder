namespace EMS.Core.Models.Mongo
{
    public class CapturedNetworkPacketMongoDocument : AuditableMongoDocument
    {
        public string Protocol { get; set; }
        public ushort HostPort { get; set; }
        public string HostAddress { get; set; }
        public ushort DestinationPort { get; set; }
        public string DestinationAddress { get; set; }
    }
}
