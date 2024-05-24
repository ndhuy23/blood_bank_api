using AutoMapper;
using Azure.Core;
using BloodBank.Data.DataAccess;
using BloodBank.Data.Dtos;
using BloodBank.Data.Entities;
using BloodBank.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Cores
{
    public interface IRequestBloodService
    {
        Task<ResultModel> CreateRequest(RequestBloodDto requestDto);
        Task<ResultModel> GetRequest(StatusRequestBlood statusSession, int page, int pageSize);
        Task<ResultModel> GetRequestByHospitalId(Guid hospitalId, int page, int pageSize);
        Task<ResultModel> UpdateRequest(Guid requestId,UpdateRequestBloodDto requestDto);
    }
    public class RequestBloodService : IRequestBloodService
    {
        private readonly BloodBankContext _db;
        private ResultModel _result;
        private readonly IMapper _mapper;

        public RequestBloodService(BloodBankContext db, IMapper mapper)
        {
            _db = db;
            _result = new ResultModel();
            _mapper = mapper;
        }

        public async Task<ResultModel> GetRequest(StatusRequestBlood statusSession, int page, int pageSize)
        {
            try
            {
                var requests = await _db.RequestBloods.Where(rq => rq.Status == statusSession).ToListAsync();

                _result.Data = requests;
                _result.IsSuccess = true;
                _result.Message = "Get successful";

            }
            catch(Exception ex)
            {
                _result.IsSuccess = false;
                _result.Message = ex.Message;
            }
            return _result;
        }

        

        public async Task<ResultModel> GetRequestByHospitalId(Guid hospitalId, int page, int pageSize)
        {
            try
            {
                var requests = await _db.RequestBloods.Where(rq => rq.HospitalId == hospitalId)
                                                       .Skip((page-1)*pageSize)
                                                       .Take(pageSize)
                                                       .ToListAsync();
                _result.Data = requests;
                _result.IsSuccess = true;
                _result.Message = "Get successful";
            }catch(Exception ex)
            {
                _result.IsSuccess = false;
                _result.Message = "Get successful";
            }
            return _result;
        }

        public async Task<ResultModel> UpdateRequest(Guid requestId, UpdateRequestBloodDto updateRequestDto)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var request = await _db.RequestBloods.FindAsync(requestId);
                    if (request == null) throw new Exception("Session is not exist");

                    var bloodIsExist = _db.Bloods.Where(b => b.HospitalId == updateRequestDto.HospitalAccept 
                                                        && b.BloodType == request.BloodType)
                                                    .Sum(b => b.Quantity) >= request.Quantity;
                    
                    if (!bloodIsExist) throw new Exception("Blood is not enough");
                    
                    request.HospitalAccept = updateRequestDto.HospitalAccept;
                    request.Status = updateRequestDto.Status;

                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    _result.IsSuccess = true;
                    _result.Message = "Update successful";

                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    _result.IsSuccess = false;
                    _result.Message = ex.Message;
                }

            }
            return _result;
        }

        public async Task<ResultModel> CreateRequest(RequestBloodDto requestDto)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var request = _mapper.Map<RequestBlood>(requestDto);

                    request.Status = StatusRequestBlood.IsWaiting;
                    await _db.RequestBloods.AddAsync(request);

                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    _result.IsSuccess = true;
                    _result.Message = "Create successful";
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
    }
}
