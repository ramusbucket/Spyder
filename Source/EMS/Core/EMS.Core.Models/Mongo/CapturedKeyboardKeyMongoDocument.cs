using EMS.Infrastructure.Common.Enums;

namespace EMS.Core.Models.Mongo
{
    public class CapturedKeyboardKeyMongoDocument : AuditableMongoDocument
    {
        public KeyboardKey KeyboardKey { get; set; }
    }
}
