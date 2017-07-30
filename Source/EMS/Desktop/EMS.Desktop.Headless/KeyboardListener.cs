namespace EMS.Desktop.Headless
{
    using System.Threading.Tasks;
    using Core.Interfaces;
    using Core.Models;
    using Infrastructure.Common.Configurations.ListenersConfigs;
    using Infrastructure.Common.Enums;
    using Infrastructure.Common.Providers;
    using Serilog;

    public class KeyboardListener : BaseListener<CapturedKeyDetails, KeyboardKey>
    {
        private IKeyboardApi keyboardApi;

        public KeyboardListener(
            IHttpClient httpClient,
            ILogger logger,
            IKeyboardApi keyboardApi,
            KeyboardListenerConfig config)
            : base(httpClient, logger, config)
        {
            this.keyboardApi = keyboardApi;
        }

        public override Task Start()
        {
            base.Start().Wait();

            this.keyboardApi.OnKeyPressed += OnKeyPressedHandler;
            return Task.Run(() => this.keyboardApi.StartListeningToKeyboard());
        }

        public override void Stop()
        {
            this.keyboardApi.StopListeningToKeyboard();
        }

        private void OnKeyPressedHandler(object sender, KeyboardKey e)
        {
            var capturedKey = new CapturedKeyDetails
            {
                KeyboardKey = e,
                CreatedOn = TimeProvider.Current.Now
            };

            this.capturedItems.Enqueue(capturedKey);
        }
    }
}
