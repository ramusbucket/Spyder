using System.Collections.Generic;
using EMS.Core.Models.DTOs;

namespace EMS.Core.Models.Mongo
{
    public class CapturedActiveProcessesMongoDocument : AuditableMongoDocument
    {
        public IEnumerable<SlimProcess> ActiveProcesses { get; set; }
    }
}
