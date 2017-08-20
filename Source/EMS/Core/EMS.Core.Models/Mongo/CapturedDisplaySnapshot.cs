using System;

namespace EMS.Web.Common.Mongo
{
    public class CapturedDisplaySnapshot : Auditable
    {
        public byte[] DisplaySnapshot { get; set; }
    }
}
