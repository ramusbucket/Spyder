using System;
using System.Threading.Tasks;
using EMS.Core.Interfaces;
using EMS.Core.Interfaces.Providers;
using EMS.Infrastructure.Common.Configurations;
using EMS.Infrastructure.Common.Enums;

namespace EMS.Core
{
    public class KeyboardApi : IKeyboardApi
    {
        private const int DefaultSleepIntervalInMilliseconds = 15;
        private IWin32ApiProvider win32ApiProvider;
        private KeyboardApiConfig config;
        private bool isListening;

        public event EventHandler<KeyboardKey> OnKeyPressed;

        public KeyboardApi(IWin32ApiProvider win32ApiProvider, KeyboardApiConfig config)
        {
            this.win32ApiProvider = win32ApiProvider;
            this.config = config;
        }

        public void StartListeningToKeyboard()
        {
            var sleepInterval = this.config != null ?
                this.config.SleepIntervalInMilliseconds :
                DefaultSleepIntervalInMilliseconds;

            this.isListening = true;

            while (this.isListening)
            {
                for (Int32 i = 0; i < 255; i++)
                {
                    var keyState = this.win32ApiProvider.GetKeyState(i);

                    if (keyState == 1 || keyState == -32767)
                    {
                        var key = (KeyboardKey)i;

                        if (key != KeyboardKey.LButton && key != KeyboardKey.RButton)
                        {
                            this.OnKeyPressed.Invoke(this, key);
                        }
                    }
                }

                Task.Delay(sleepInterval).Wait();
            }
        }

        public void StopListeningToKeyboard()
        {
            this.isListening = false;
        }
    }
}
