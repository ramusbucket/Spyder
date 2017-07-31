namespace EMS.Infrastructure.Common.Configurations
{
    public class ProcessApiConfig
    {
        public TimerConfig ForegroundProcessChangedTimerConfig { get; set; }

        public TimerConfig ActiveProcessesChangedTimerConfig { get; set; }
    }
}
