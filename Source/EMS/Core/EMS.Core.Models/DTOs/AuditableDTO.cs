using System;

namespace EMS.Core.Models.DTOs
{
    public class AuditableDto
    {
        public string UserId { get; set; }

        public string SessionId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
