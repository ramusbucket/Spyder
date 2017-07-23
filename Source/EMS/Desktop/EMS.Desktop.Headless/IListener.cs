using System.Threading.Tasks;

namespace EMS.Desktop.Headless
{
    public interface IListener
    {
        Task Start();

        void Stop();
    }
}
