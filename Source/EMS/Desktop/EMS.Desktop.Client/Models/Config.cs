using EMS.Infrastructure.Common.Configurations;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;

namespace EMS.Desktop.Client.Models
{
    public class Config
    {
        public static Config Default = new Config();

        public UrisConfig UrisConfig { get; set; }
        public OperatingSystemAPIsConfig OperatingSystemAPIsConfig { get; set; }
        public ListenersConfig ListenersConfig { get; set; }
    }

    public class UrisConfig
    {
        public string BaseServiceUri { get; set; }
        public string RegisterUserUri { get; set; }
        public string LoginUserUri { get; set; }
    }

    public class OperatingSystemAPIsConfig
    {
        public KeyboardApiConfig KeyboardApiConfig { get; set; }
        public DisplayApiConfig DisplayApiConfig { get; set; }
        public CameraApiConfig CameraApiConfig { get; set; }
        public ProcessApiConfig ProcessApiConfig { get; set; }
    }

    public class ListenersConfig
    {
        public KeyboardListenerConfig KeyboardListenerConfig { get; set; }
        public DisplayListenerConfig DisplayListenerConfig { get; set; }
        public CameraListenerConfig CameraListenerConfig { get; set; }
        public ActiveProcessesListenerConfig ActiveProcessesListenerConfig { get; set; }
        public ForegroundProcessListenerConfig ForegroundProcessListenerConfig { get; set; }
        public NetworkListenerConfig NetworkListenerConfig { get; set; }
    }
}
