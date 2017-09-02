using EMS.Infrastructure.Statistics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMS.Infrastructure.Workers
{
    /// <summary>
    /// Worker type which implements the pattern:
    /// 1. Pull item from input source
    /// 2. Process item
    /// 3. Push item to output source
    /// </summary>
    public abstract class Worker<TIn, TOut>
    {
        private readonly IStatisticsCollector _statisticsCollector;
        private readonly CancellationToken _cancellationToken;

        protected Worker(IStatisticsCollector statisticsCollector, CancellationToken cancellationToken)
        {
            _statisticsCollector = statisticsCollector;
            _cancellationToken = cancellationToken;
        }

        public async Task Run()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Measure and Log execution time
                    var inputItem = await PullNextAsync();

                    // Measure and Log execution time
                    var outputItem = await ProcessAsync(inputItem);

                    // Measure and Log execution time
                    await PushAsync(outputItem);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }
            }
        }

        public abstract Task<TIn> PullNextAsync();

        public abstract Task<TOut> ProcessAsync(TIn input);

        public abstract Task PushAsync(TOut output);
    }
}
