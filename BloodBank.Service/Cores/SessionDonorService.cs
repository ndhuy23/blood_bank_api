using AutoMapper;
using BloodBank.Data.DataAccess;
using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Hospital;
using BloodBank.Data.Dtos.SessionDonor;
using BloodBank.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Cores
{
    public interface ISessionDonorService
    {
        Task<ResultModel> CreateSession(SessionDonorDto sessionDto);
        Task<ResultModel> DeleteById(Guid sessionId);
        Task<ResultModel> GetByActivityId(Guid activityId, PagingModel session);
        Task<ResultModel> GetByDonorId(Guid donorId);
        Task<ResultModel> Update(Guid sessionId, UpdateSessionDto sessionDto);
    }
    public class SessionDonorService : ISessionDonorService
    {
        private readonly BloodBankContext _db;
        private ResultModel _result;
        private readonly IMapper _mapper;

        public SessionDonorService(BloodBankContext db, IMapper mapper)
        {
            _db = db;
            _result = new ResultModel();
            _mapper = mapper;
        }
        public async Task<bool> CheckDate(Activity activity, Guid donorId)
        {
            try
            {
                bool session = false;
                bool historyCheck = false;

                var historyRecently = await _db.Histories.Where(h => h.DonorId == donorId).OrderByDescending(h => h.DonationDate).FirstOrDefaultAsync();

                if (historyRecently == null || activity.DateActivity > historyRecently.DonationDate + TimeSpan.FromDays(60))
                {
                    historyCheck = true;
                }
                var sessionRecently = await _db.SessionDonors.Where(ss => ss.DonorId == donorId).OrderByDescending(ss => ss.Activity.DateActivity).FirstOrDefaultAsync();
                
                if (sessionRecently == null)
                {
                    session = true;
                }
                else 
                {
                    var activityOfSession = await _db.Activities.FindAsync(sessionRecently.ActivityId);
                    if (Math.Abs((activity.DateActivity - activityOfSession.DateActivity).TotalDays) > 60) session = true;
                    
                }

                return historyCheck && session;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ResultModel> CreateSession(SessionDonorDto sessionDto)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                { 

                    var activity = await _db.Activities.FindAsync(sessionDto.ActivityId);
                    
                    var isExisted = await CheckDate(activity, sessionDto.DonorId);
                    
                    if (!isExisted) throw new Exception("You donated blood or had register donation two month ago");
                    
                    if (activity.NumberIsRegistration == activity.Quantity) throw new Exception("Quantity register activity is full");
                    activity.NumberIsRegistration++;
                    
                    var session = _mapper.Map<SessionDonor>(sessionDto);
                    _db.SessionDonors.Add(session);
                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    _result.IsSuccess = true;
                    _result.Message = "Create successful";
                    return _result;
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

        public async Task<ResultModel> DeleteById(Guid sessionId)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var session = await _db.SessionDonors.FindAsync(sessionId);
                    if (session == null) throw new Exception("Session is not exist");
                    var activity = await _db.Activities.FindAsync(session.ActivityId);
                    activity.NumberIsRegistration--;
                    _db.SessionDonors.Remove(session);
                    
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    _result.IsSuccess = true;
                    _result.Message = "Delete successful";

                }catch(Exception ex)
                {
                    transaction.Rollback();
                    _result.IsSuccess = false;
                    _result.Message = ex.Message;
                }
            }
            return _result;
        }

        public async Task<ResultModel> GetByActivityId(Guid activityId, PagingModel paging)
        {
            try
            {
                var session = await _db.SessionDonors.Where(ss => ss.ActivityId == activityId)
                                                    .Skip((paging.Page -1)*paging.PageSize)
                                                    .Take(paging.PageSize)
                                                    .ToListAsync();
                var totalCount = await _db.SessionDonors.Where(ss => ss.ActivityId == activityId).CountAsync();
                paging.Data = session;
                paging.TotalCount = totalCount;

                _result.Data = paging;
                _result.IsSuccess = true;
                _result.Message = "";
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Message = ex.Message;
            }
            return _result;
        }

        public async Task<ResultModel> GetByDonorId(Guid donorId)
        {
            try
            {
                var session = await _db.SessionDonors.Where(ss => ss.DonorId == donorId)
                                                    .ToListAsync();

                _result.Data = session;
                _result.IsSuccess = true;
                _result.Message = "";
            }
            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Message = ex.Message;
            }
            return _result;
        }

        public async Task<ResultModel> Update(Guid sessionId, UpdateSessionDto sessionDto)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var session = await _db.SessionDonors.FindAsync(sessionId);
                    if (session == null) throw new Exception("Session is not exist");

                    _mapper.Map(sessionDto, session);

                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    _result.IsSuccess = true;
                    _result.Message = "Update successful";

                }catch(Exception ex)
                {
                    transaction.Rollback();
                    _result.IsSuccess = true;
                    _result.Message = "Update successful";
                }
            }
            return _result;
        }
    }
}
