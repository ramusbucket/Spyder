namespace EMS.Core.Models.Mongo
{
    public class CapturedCameraSnapshotMongoDocument : AuditableMongoDocument
    {
        public byte[] CameraSnapshot { get; set; }
    }
}
