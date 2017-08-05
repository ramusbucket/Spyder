using System.Diagnostics;

namespace EMS.Core.Models
{
    public class CapturedForegroundProcessDTO : Auditable
    {
        public SlimProcess CapturedForegroundProcess { get; set; }
    }
}
