using EMS.Core.Models.DTOs;
using System.Diagnostics;

namespace EMS.Core.Models
{
    public class CapturedForegroundProcessDTO : BaseDTO
    {
        public SlimProcess CapturedForegroundProcess { get; set; }
    }
}
