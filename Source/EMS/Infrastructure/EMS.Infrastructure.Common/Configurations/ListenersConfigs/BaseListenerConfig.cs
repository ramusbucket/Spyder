using System;
using System.Collections.Generic;
using System.Linq;

namespace EMS.Infrastructure.Common.Configurations.ListenersConfigs
{
    public class BaseListenerConfig
    {
        public TimerConfig SendCapturedItemsTimerConfig { get; set; }

        public int SendCapturedItemsThreshold { get; set; }

        public string SendCapturedItemsDestinationUri { get; set; }

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
