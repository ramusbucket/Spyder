using Easy.Common;
using Easy.Common.Interfaces;
using EMS.Core;
using EMS.Core.Interfaces;
using EMS.Core.Interfaces.Providers;
using EMS.Core.Providers;
using EMS.Desktop.Client.Listeners;
using EMS.Desktop.Client.Models;
using EMS.Infrastructure.Common.Configurations;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.DependencyInjection;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using Serilog;
using Serilog.Formatting.Json;

namespace EMS.Desktop.Client
{
    public static class DependenciesRegister
    {
        public static IInjector RegisterDependencies(Config config)
        {
            var injector = UnityInjector.Instance;

            RegisterLogger(injector);
            RegisterHelpersAndProviders(injector, config);

            RegisterOperatingSystemAPIsConfigs(injector, config);
            RegisterOperatingSystemAPIs(injector);

            RegisterListenersConfigs(injector, config);
            RegisterListeners(injector);

            return injector;
        }

        private static void RegisterListenersConfigs(IInjector injector, Config config)
        {
            var listenersConfig = config.ListenersConfig;

            injector
                .RegisterInstance<CameraListenerConfig>(listenersConfig.CameraListenerConfig)
                .RegisterInstance<DisplayListenerConfig>(listenersConfig.DisplayListenerConfig)
                .RegisterInstance<NetworkListenerConfig>(listenersConfig.NetworkListenerConfig)
                .RegisterInstance<KeyboardListenerConfig>(listenersConfig.KeyboardListenerConfig)
                .RegisterInstance<ActiveProcessesListenerConfig>(listenersConfig.ActiveProcessesListenerConfig)
                .RegisterInstance<ForegroundProcessListenerConfig>(listenersConfig.ForegroundProcessListenerConfig);
        }

        private static void RegisterListeners(IInjector injector)
        {
            injector
                .Register<IListener, CameraListener>(nameof(CameraListener))
                .Register<IListener, DisplayListener>(nameof(DisplayListener))
                .Register<IListener, KeyboardListener>(nameof(KeyboardListener))
                .Register<IListener, NetworkPacketListener>(nameof(NetworkPacketListener))
                .Register<IListener, ActiveProcessesListener>(nameof(ActiveProcessesListener))
                .Register<IListener, ForegroundProcessListener>(nameof(ForegroundProcessListener));
        }

        private static void RegisterOperatingSystemAPIs(IInjector injector)
        {
            injector
                .Register<ICameraApi, CameraApi>()
                .Register<IDisplayApi, DisplayApi>()
                .Register<IProcessApi, ProcessApi>()
                .Register<INetworkApi, NetworkApi>()
                .Register<IKeyboardApi, KeyboardApi>();
        }

        private static void RegisterOperatingSystemAPIsConfigs(IInjector injector, Config config)
        {
            var operatingSystemAPIsConfig = config.OperatingSystemAPIsConfig;

            injector
                .RegisterInstance<CameraApiConfig>(operatingSystemAPIsConfig.CameraApiConfig)
                .RegisterInstance<DisplayApiConfig>(operatingSystemAPIsConfig.DisplayApiConfig)
                .RegisterInstance<ProcessApiConfig>(operatingSystemAPIsConfig.ProcessApiConfig)
                .RegisterInstance<KeyboardApiConfig>(operatingSystemAPIsConfig.KeyboardApiConfig);
        }

        private static void RegisterHelpersAndProviders(IInjector injector, Config config)
        {
            injector
                .RegisterInstance<IRestClient>(new RestClient())
                .Register<IWin32ApiProvider, Win32ApiProvider>();
        }

        private static void RegisterLogger(IInjector injector)
        {
            var delimiter = new string('*', 40);
            var formatter = new JsonFormatter(closingDelimiter: delimiter);
            var logsFilePath = @"..\..\Logs\Serilog.txt";

            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(formatter, logsFilePath)
                .CreateLogger();

            injector.RegisterInstance<ILogger>(logger);
        }
    }
}
