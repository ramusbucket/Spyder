using EMS.Infrastructure.Common.Enums;

namespace EMS.Core.Models
{
    public class CapturedKeyDetailsDTO : Auditable
    {
        public KeyboardKey KeyboardKey { get; set; }
    }
}
