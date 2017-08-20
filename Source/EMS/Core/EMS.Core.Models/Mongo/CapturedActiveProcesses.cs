using EMS.Core.Models.DTOs;
using System.Collections.Generic;

namespace EMS.Web.Common.Mongo
{
    public class CapturedActiveProcesses : Auditable
    {
        public IEnumerable<SlimProcess> ActiveProcesses { get; set; }
    }
}
