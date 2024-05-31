using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Utils.BackGround
{
    public class ScopedActivityHostedService : BackgroundService
    {
        private readonly ILogger<ScopedActivityHostedService> _logger;
        private IScheduler _scheduler;
        private IServiceProvider _serviceProvider;
        public ScopedActivityHostedService(IServiceProvider serviceProvider, ILogger<ScopedActivityHostedService> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            await UpdateActivityStatus(cancellationToken);

        }

        private async Task UpdateActivityStatus(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var activityWorker = scope.ServiceProvider.GetRequiredService<IActivityWorker>();
                await activityWorker.DoWork(cancellationToken);
            }
        }
        
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
