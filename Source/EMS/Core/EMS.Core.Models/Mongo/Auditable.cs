using System;

namespace EMS.Web.Common.Mongo
{
    public class Auditable
    {
        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
