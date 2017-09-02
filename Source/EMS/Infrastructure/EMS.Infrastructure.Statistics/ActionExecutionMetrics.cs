namespace EMS.Infrastructure.Statistics
{
    public class ActionExecutionMetrics
    {
        /// <summary>
        /// The application which executed the action.
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// The server which hosts the application.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The action which is being measured.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// The action execution time in milliseconds.
        /// </summary>
        public long ActionExecutionTime { get; set; }
    }
}
