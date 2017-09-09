using System.Threading.Tasks;
using Easy.Common.Interfaces;
using EMS.Core.Interfaces;
using EMS.Core.Models.DTOs;
using EMS.Desktop.Client.Models;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Enums;
using EMS.Infrastructure.Common.Providers;
using Serilog;

namespace EMS.Desktop.Client.Listeners
{
    public class KeyboardListener : BaseListener<CapturedKeyDetailsDto>
    {
        private IKeyboardApi keyboardApi;

        public KeyboardListener(
            IRestClient httpClient,
            ILogger logger,
            IKeyboardApi keyboardApi,
            KeyboardListenerConfig config)
            : base(httpClient, logger, config)
        {
            this.keyboardApi = keyboardApi;
        }

        public override async Task Start()
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
            var capturedItem = new CapturedKeyDetailsDto
            {
                KeyboardKey = e,
                CreatedOn = TimeProvider.Current.Now,
                SessionId = Identity.SessionId
            };

            this.capturedItems.Enqueue(capturedItem);
        }
    }
}
