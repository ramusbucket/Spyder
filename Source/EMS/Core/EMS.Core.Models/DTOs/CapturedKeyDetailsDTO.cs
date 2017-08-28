using EMS.Infrastructure.Common.Enums;

namespace EMS.Core.Models.DTOs
{
    public class CapturedKeyDetailsDto : AuditableDto
    {
        public KeyboardKey KeyboardKey { get; set; }
    }
}
