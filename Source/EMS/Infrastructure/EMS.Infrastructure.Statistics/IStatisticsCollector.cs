using System;
using System.Threading.Tasks;

namespace EMS.Infrastructure.Statistics
{
    public interface IStatisticsCollector
    {
        void Send<T>(T stats);

        Task SendWithAck<T>(T stats);

        T Measure<T>(Func<T> action, string actionName);

        void Measure(Action action, string actionName);

        Task Measure(Func<Task> action, string actionName);

        Task<T> Measure<T>(Func<Task<T>> action, string actionName);

        Task<T> MeasureWithAck<T>(Func<T> action, string actionName);

        Task MeasureWithAck(Action action, string actionName);

        Task MeasureWithAck(Func<Task> action, string actionName);

        Task<T> MeasureWithAck<T>(Func<Task<T>> action, string actionName);
    }
}