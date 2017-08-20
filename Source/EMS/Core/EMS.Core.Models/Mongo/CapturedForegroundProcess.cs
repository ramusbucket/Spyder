using EMS.Core.Models.DTOs;

namespace EMS.Web.Common.Mongo
{
    public class CapturedForegroundProcess : Auditable
    {
        public SlimProcess ForegroundProcess { get; set; }
    }
}
