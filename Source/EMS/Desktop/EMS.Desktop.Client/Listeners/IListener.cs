using System.Threading.Tasks;

namespace EMS.Desktop.Client
{
    public interface IListener
    {
        Task Start();

        void Stop();
    }
}
