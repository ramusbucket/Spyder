using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EMS.Infrastructure.Common.Configurations;
using EMS.Infrastructure.Common.Configurations.ListenersConfigs;
using Polly;
using Serilog;
using Easy.Common.Interfaces;
using System.Net.Http;
using Easy.Common;
using Newtonsoft.Json;
using System.Text;
using EMS.Core.Models.DTOs;
using EMS.Desktop.Client.Models;

namespace EMS.Desktop.Client
{
    public abstract class BaseListener<T> : IListener
        where T : class
    {
        protected ILogger logger;
        protected IRestClient httpClient;
        protected ConcurrentQueue<T> capturedItems;
        protected Timer sendCapturedKeysTimer;
        protected BaseListenerConfig config;
        protected Uri sendCapturedItemsUri;

        public BaseListener(
            IRestClient httpClient,
            ILogger logger,
            BaseListenerConfig config)
        {
            this.config = config;
            this.logger = logger;
            this.httpClient = httpClient;
            this.capturedItems = new ConcurrentQueue<T>();
            this.sendCapturedItemsUri = new Uri(this.config.SendCapturedItemsDestinationUri);
        }

        public virtual Task Start()
        {
            this.sendCapturedKeysTimer = this.InitializeTimer(
                this.config.SendCapturedItemsTimerConfig,
                async (_) => await this.SendCapturedItems());

            return Task.FromResult<object>(null);
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
                        var httpRequest = new HttpRequestMessage(HttpMethod.Post, sendCapturedItemsUri);
                        httpRequest.Content = new JSONContent(JsonConvert.SerializeObject(itemsToSend), Encoding.UTF8);
                        httpRequest.Headers.Add("Authorization", $"Bearer {Identity.AuthToken.AccessToken}");

                        var response = await this.httpClient.SendAsync(httpRequest);
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
