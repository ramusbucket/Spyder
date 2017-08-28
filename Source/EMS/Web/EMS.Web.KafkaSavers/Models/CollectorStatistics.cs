using System.Collections.Concurrent;

namespace EMS.Web.KafkaSavers.Models
{
    public static class CollectorStatistics
    {
        public static ConcurrentDictionary<string, long> Counters { get; } 
            = new ConcurrentDictionary<string, long>();
    }
}