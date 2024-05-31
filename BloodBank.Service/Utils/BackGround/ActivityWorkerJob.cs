using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Utils.BackGround
{
    public class ActivityWorkerJob : IJob
    {
        public IServiceProvider _serviceProvider { get; }
        private readonly ILogger<ActivityWorkerJob> _logger;
        public ActivityWorkerJob(IServiceProvider services,
            ILogger<ActivityWorkerJob> logger)
        {
            _serviceProvider = services;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Scheduling job with trigger: ");
            using (var scope = _serviceProvider.CreateScope())
            {
                var activityWorker = scope.ServiceProvider.GetRequiredService<IActivityWorker>();
                await activityWorker.DoWork(context.CancellationToken);
            }
        }
    }
}
