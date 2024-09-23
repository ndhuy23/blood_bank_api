using AutoMapper;
using BloodBank.Data.DataAccess;
using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Activity;
using BloodBank.Data.Entities;
using BloodBank.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Cores
{
    public interface IActivityService
    {
        Task<ResultModel> AddActivity(ActivityDto activity);
        Task<ResultModel> Delete(Guid activityId);
        Task<ResultModel> GetActivityFromTo(Guid? hospitalId, DateTime? cursor, DateTimeOffset? from, DateTimeOffset? to, int pageSize, StatusActivity? status);
        Task<ResultModel> GetActivity(Guid? hospitalId, StatusActivity? status);

        Task<ResultModel> Update(Guid hospitalId, ActivityDto activity);
        Task<ResultModel> GetActivityById(Guid activityId);
    }
    public class ActivityService : IActivityService
    {
        private readonly BloodBankContext _db;
        private ResultModel _result;
        private readonly IMapper _mapper;
        public ActivityService(BloodBankContext db, IMapper mapper)
        {
            _db = db;
            _result = new ResultModel();
            _mapper = mapper;
        }

        public async Task<ResultModel> AddActivity(ActivityDto activity)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var activityNew = _mapper.Map<Activity>(activity);
                    var hospital = await _db.Hospitals.FindAsync(activityNew.HospitalId);
                    if (hospital == null) throw new Exception("Not valid hospital");


                    activityNew.Hospital = hospital;
                    activityNew.NumberIsRegistration = 0;

                    if (activity.DateActivity.Date == DateTime.Today) activityNew.Status = StatusActivity.IsGoing;
                    _db.Activities.Add(activityNew);
                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    _result.IsSuccess = true;
                    _result.Message = "Create activity successful";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _result.IsSuccess = false;
                    _result.Message = ex.Message;
                }
            }
            
            return _result;
        }
        public async Task<ResultModel> GetActivityById(Guid activityId)
        {
            try
            {
                var activity = await _db.Activities.FindAsync(activityId);
                if (activity == null) throw new Exception("Activity is not exist");
                _result.Data = activity;
                _result.IsSuccess = true;
                _result.Message = "Get successful";
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Message = ex.Message;
            }
            return _result;
        }
        public async Task<ResultModel> GetActivityFromTo(Guid? hospitalId, DateTime? cursor, DateTimeOffset? from, DateTimeOffset? to, int pageSize, StatusActivity? status)
        {
            try
            {
                var query = _db.Activities.AsQueryable();
                if (hospitalId.HasValue)
                {
                    query = query.Where(r => r.Id == hospitalId).OrderBy(r => r.DateActivity);
                }
                if (cursor.HasValue)
                {
                    query = query.Where(r => r.DateActivity > cursor).OrderBy(r => r.DateActivity);
                }
                else
                {
                    query = query.OrderBy(r => r.DateActivity);
                }
                
                if (from.HasValue)
                {
                    query = query.Where(r => r.DateActivity.Date >= from.Value.Date);
                }

                if (to.HasValue)
                {
                    query = query.Where(r => r.DateActivity.Date <= to.Value.Date);
                }
                if (status.HasValue)
                {
                    query = query.Where(r => r.Status == status);
                }
                query = query.Take(pageSize);

                _result.Data = query.ToList();
                _result.IsSuccess = true;
                _result.Message = "Get activity successful";

                return _result;
            }
            catch(Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;
            
        }

        public async Task<ResultModel> Update(Guid activityId, ActivityDto activityDto)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _result = await GetActivityById(activityId);

                    _mapper.Map(activityDto, _result.Data);

                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    _result.IsSuccess = true;
                    _result.Message = "Update successful";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _result.IsSuccess = false;
                    _result.Message = ex.Message;
                }
            }
            return _result;
        }

        public async Task<ResultModel> Delete(Guid activityId)
        {

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _result = await GetActivityById(activityId);
                    _db.Activities.Remove((Activity)_result.Data);

                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    _result.Data = null;
                    _result.IsSuccess = true;
                    _result.Message = "Delete successful";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _result.IsSuccess = false;
                    _result.Message = ex.Message;
                }
            }
            return _result;
        }

        public async Task<ResultModel> GetActivity(Guid? hospitalId, StatusActivity? status)
        {
            try
            {
                var query = _db.Activities.AsQueryable();
                if (hospitalId.HasValue)
                {
                    query = query.Where(r => r.HospitalId == hospitalId).OrderBy(a => a.DateActivity);
                }
                if (status.HasValue)
                {
                    query = query.Where(a => a.Status == status).OrderBy(a => a.DateActivity);
                }
                

                _result.Data = query.ToList();
                _result.IsSuccess = true;
                _result.Message = "Get activity successful";

                return _result;
            }
            catch (Exception e)
            {
                _result.IsSuccess = false;
                _result.Message = e.Message;
            }
            return _result;
            throw new NotImplementedException();
        }
    }
}
