using System.Diagnostics;

namespace EMS.Core.Models
{
    public class CapturedForegroundProcessDetails : Auditable
    {
        public SlimProcess CapturedForegroundProcess { get; set; }
    }
}
