using EMS.Core.Models.DTOs;

namespace EMS.Core.Models.Mongo
{
    public class CapturedForegroundProcessMongoDocument : AuditableMongoDocument
    {
        public SlimProcess ForegroundProcess { get; set; }
    }
}
