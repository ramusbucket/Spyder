namespace EMS.Infrastructure.Common.Configurations
{
    public class ProcessAPIConfig
    {
        public TimerConfig ForegroundProcessChangedTimerConfig { get; set; }

        public TimerConfig ActiveProcessesChangedTimerConfig { get; set; }
    }
}
