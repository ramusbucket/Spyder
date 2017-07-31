using System.Diagnostics;

namespace EMS.Core.Models
{
    public class CapturedForegroundProcessDetails : Auditable
    {
        public Process CapturedForegroundProcess { get; set; }
    }
}
