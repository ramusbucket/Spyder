using System;

namespace EMS.Core.Models
{
    public class CapturedDisplaySnapshotDetails : Auditable
    {
        public byte[] DisplaySnapshot { get; set; }
    }
}
