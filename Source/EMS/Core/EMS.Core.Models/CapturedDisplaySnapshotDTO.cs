using System;

namespace EMS.Core.Models
{
    public class CapturedDisplaySnapshotDTO : Auditable
    {
        public byte[] DisplaySnapshot { get; set; }
    }
}
