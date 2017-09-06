using System;
using System.Threading.Tasks;

namespace EMS.Infrastructure.Statistics
{
    public interface IStatisticsCollector
    {
        void Send<T>(T stats, string topic);

        Task SendWithAck<T>(T stats, string topic);

        T Measure<T>(Func<T> action, string actionName, string topic);

        void Measure(Action action, string actionName, string topic);

        Task Measure(Func<Task> action, string actionName, string topic);

        Task<T> Measure<T>(Func<Task<T>> action, string actionName, string topic);

        Task<T> MeasureWithAck<T>(Func<T> action, string actionName, string topic);

        Task MeasureWithAck(Action action, string actionName, string topic);

        Task MeasureWithAck(Func<Task> action, string actionName, string topic);

        Task<T> MeasureWithAck<T>(Func<Task<T>> action, string actionName, string topic);
    }
}