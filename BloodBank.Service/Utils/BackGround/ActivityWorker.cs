using BloodBank.Data.DataAccess;
using BloodBank.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Utils.BackGround
{
    public interface IActivityWorker
    {
        Task DoWork(CancellationToken stoppingToken);
    }
    public class ActivityWorker : IActivityWorker
    {
        private readonly BloodBankContext _db;
        private ILogger<ActivityWorker> _logger;
        public ActivityWorker(BloodBankContext db, ILogger<ActivityWorker> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task DoWork(CancellationToken stoppingToken)
        {
            try
            {
                // Lấy danh sách các hoạt động chưa hoàn thành
                var activities = await _db.Activities.Where(a => (a.Status == StatusActivity.IsWaiting ||
                                                                 a.Status == StatusActivity.IsGoing) && a.DateActivity.Date <= DateTime.Now.Date)
                                                    .ToListAsync();

                // Kiểm tra và cập nhật trạng thái
                foreach (var activity in activities)
                {
                    _logger.LogInformation($"{activity.DateActivity} aaaaaa");
                    if (activity.Status == StatusActivity.IsGoing)
                    {
                        activity.Status = StatusActivity.Done;
                    }
                    else
                    {
                        activity.Status = StatusActivity.IsGoing;
                    }
                    _db.Update(activity);
                }

               await _db.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
            

        }

        
    }
}
