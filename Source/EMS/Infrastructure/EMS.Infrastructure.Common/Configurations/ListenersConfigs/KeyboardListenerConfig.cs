using System;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Infrastructure.Common.Configurations.ListenersConfigs
{
    public class KeyboardListenerConfig
    {
        public int CapturedKeysThreshold { get; set; }

        public string DestinationUri { get; set; }

        public List<int> RetrySleepDurationsInMilliseconds { get; set; }

        public TimeSpan[] RetrySleepDurations
        {
            get
            {
                return this.RetrySleepDurationsInMilliseconds
                    .Select(x => TimeSpan.FromMilliseconds(x))
                    .ToArray();
            }
        }
    }
}
