namespace EMS.Core.Models.Mongo
{
    public class CapturedDisplaySnapshotMongoDocument : AuditableMongoDocument
    {
        public byte[] DisplaySnapshot { get; set; }
    }
}
