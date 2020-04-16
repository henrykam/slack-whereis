using HenryKam.SlackWhereIs.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HenryKam.SlackWhereIs.Application
{
    public class BotProcessingService : BackgroundService
    {
        private readonly ILogger<BotProcessingService> _logger;
        private IBackgroundTaskQueue _taskQueue { get; }
        private IServiceProvider _services { get; }

        public BotProcessingService(IServiceProvider services, IBackgroundTaskQueue taskQueue, ILogger<BotProcessingService> logger)
        {
            _taskQueue = taskQueue;
            _logger = logger;
            _services = services;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Bot Processing Service is starting.");
            CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var token = cts.Token;
            while (!token.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(token);

                try
                {
                    _logger.LogDebug($"Executing {nameof(workItem)}.");
                    await workItem(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                       "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }

            _logger.LogInformation("Bot Processing Service is stopping.");
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await Task.CompletedTask;
        }

    }
}
