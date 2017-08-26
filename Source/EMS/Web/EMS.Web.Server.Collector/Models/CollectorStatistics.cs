﻿using System.Collections.Concurrent;

namespace EMS.Web.Server.Collector.Models
{
    public static class CollectorStatistics
    {
        public static ConcurrentDictionary<string, long> Counters { get; } 
            = new ConcurrentDictionary<string, long>();
    }
}