using EMS.Core.Models.DTOs;

namespace EMS.Core.Models.Mongo
{
    public class CapturedActiveProcessesMongoDocument : AuditableMongoDocument
    {
        public SlimProcess ActiveProcess { get; set; }
    }
}
