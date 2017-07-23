using System;

namespace EMS.Infrastructure.Common.Providers
{
    public class DefaultTimeProvider : TimeProvider
    {
        private static Lazy<DefaultTimeProvider> instance = 
            new Lazy<DefaultTimeProvider>(
                () => new DefaultTimeProvider());

        private DefaultTimeProvider()
        {
        }

        public static DefaultTimeProvider Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public override DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        public override DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
