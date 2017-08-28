using System;

namespace EMS.Web.MongoSavers.Models
{
    public class ServiceStatistics
    {
        public DateTime StartDate { get; set; }

        public DateTime LastPollDate { get; set; }

        public DateTime LastExceptionDate { get; set; }

        public DateTime LastProcessedItemDate { get; set; }

        public DateTime LastReceivedMessageDate { get; set; }

        public ulong ProcessedItemsCount { get; set; }

        public Exception LastException { get; set; }

        public string LastExceptionMessage => LastException?.Message;
    }
}
