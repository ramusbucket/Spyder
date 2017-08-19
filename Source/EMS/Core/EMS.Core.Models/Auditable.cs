using System;

namespace EMS.Core.Models
{
    public class Auditable
    {
        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
