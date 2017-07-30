using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EMS.Infrastructure.Common.Configurations;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using EMS.Infrastructure.Common.Providers;
using Polly;
using Serilog;

namespace EMS.Desktop.Headless
{
    public abstract class BaseListener<T, K> : IListener
        where T : class
    {
        protected ILogger logger;
        protected IHttpClient httpClient;
        protected ConcurrentQueue<T> capturedItems;
        protected Timer sendCapturedKeysTimer;
        protected BaseListenerConfig config;

        public BaseListener(
            IHttpClient httpClient,
            ILogger logger,
            BaseListenerConfig config)
        {
            this.config = config;
            this.logger = logger;
            this.httpClient = httpClient;
            this.capturedItems = new ConcurrentQueue<T>();
        }

        public virtual Task Start()
        {
            this.sendCapturedKeysTimer = this.InitializeTimer(
                this.config.SendCapturedItemsTimerConfig,
                async (_) => await this.SendCapturedItems());

            return null;
        }

        public abstract void Stop();

        protected virtual Timer InitializeTimer(TimerConfig timerConfig, TimerCallback callback)
        {
            var dueTime = timerConfig.DueTime;
            var period = timerConfig.Period;

            return new Timer(
                callback,
                null,
                dueTime,
                period);
        }

        protected virtual async Task SendCapturedItems()
        {
            var capturedItemsCount = this.capturedItems.Count;

            if (capturedItemsCount >= this.config.SendCapturedItemsThreshold)
            {
                var hasItem = true;
                var item = default(T);
                var itemsToSend = new List<T>(capturedItemsCount + 1);

                for (int i = 0; i < capturedItemsCount && hasItem; i++)
                {
                    hasItem = this.capturedItems.TryDequeue(out item);

                    if (hasItem)
                    {
                        itemsToSend.Add(item);
                    }
                }

                await Policy
                  .Handle<Exception>()
                  .WaitAndRetryAsync(3, (x) => TimeSpan.FromSeconds(x), OnSendCapturedItemsRetry)
                  .ExecuteAsync(
                    async () =>
                    {
                        var response = await this.httpClient.PostAsJsonAsync(
                            this.config.SendCapturedItemsDestinationUri,
                            itemsToSend);
                        response.EnsureSuccessStatusCode();
                    });
            }
        }

        protected virtual async Task OnSendCapturedItemsRetry(Exception exc, TimeSpan waitTime, int retryCount, Context context)
        {
            this.logger.Warning(exc.ToString());

            await Task.Yield();
        }
    }
}
