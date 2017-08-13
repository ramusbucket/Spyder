namespace EMS.Desktop.Client
{
    using System.Threading.Tasks;
    using Core.Interfaces;
    using Core.Models;
    using Infrastructure.Common.Configurations.ListenersConfigs;
    using Infrastructure.Common.Enums;
    using Infrastructure.Common.Providers;
    using Serilog;

    public class KeyboardListener : BaseListener<CapturedKeyDetailsDTO, KeyboardKey>
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

        public async override Task Start()
        {
            await base.Start();

            this.keyboardApi.OnKeyPressed += OnKeyPressedHandler;

            await Task.Run(
                () => this.keyboardApi.StartListeningToKeyboard());
        }

        public override void Stop()
        {
            this.keyboardApi.StopListeningToKeyboard();
        }

        private void OnKeyPressedHandler(object sender, KeyboardKey e)
        {
            var capturedItem = new CapturedKeyDetailsDTO
            {
                KeyboardKey = e,
                CreatedOn = TimeProvider.Current.Now
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
