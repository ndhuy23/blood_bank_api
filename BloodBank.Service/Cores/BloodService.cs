using AutoMapper;
using BloodBank.Data.DataAccess;
using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Donor;
using BloodBank.Data.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Cores
{
    public interface IBloodService
    {
        Task<ResultModel> CreateBlood(BloodDto bloodDto);
        Task<ResultModel> DeleteById(Guid donorId);
        Task<ResultModel> GetBlood(PagingModel donorDto);
        Task<ResultModel> GetById(Guid donorId);
        Task<ResultModel> Update(Guid donorId, BloodDto donorDto);
        Task<ResultModel> GetBloodByHospitalId(Guid hospitalId);

        Task<ResultModel> GetBloods(Guid hospitalId, string bloodType);
        Task<ResultModel> ExportBloods(Guid requestId,Guid hospitalNewId, List<Guid> bloodIds);
    }
    public class BloodService : IBloodService
    {
        private readonly BloodBankContext _db;
        private ResultModel _result;
        private readonly IMapper _mapper;
        public BloodService(BloodBankContext db, IMapper mapper)
        {
            _db = db;
            _result = new ResultModel();
            _mapper = mapper;
        }

        public async Task<ResultModel> CreateBlood(BloodDto bloodDto)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var blood = _mapper.Map<Blood>(bloodDto);
                    
                    blood.ExpiryDate = DateTime.Now.AddDays(35);

                    _db.Bloods.Add(blood);
                    await _db.SaveChangesAsync();
                    transaction.Commit();

                    _result.Data = blood;
                    _result.IsSuccess = true;
                    _result.Message = "Create Blood successful";
                }catch(Exception ex)
                {
                    transaction.Rollback();

                    _result.IsSuccess = false;
                    _result.Message = ex.Message;
                }
            }
            return _result;
        }

        public Task<ResultModel> DeleteById(Guid donorId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel> GetBlood(PagingModel donorDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultModel> GetById(Guid bloodId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel> Update(Guid donorId, BloodDto donorDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultModel> GetBloodByHospitalId(Guid hospitalId)
        {
            try
            {
                var bloodTypeSummary = await _db.Bloods.Where(bl => bl.HospitalId == hospitalId).GroupBy(b => b.BloodType)
                               
                               .Select(g => new
                               {
                                   BloodType = g.Key,
                                   TotalBloodQuantity = g.Sum(b => b.Quantity)
                               })
                               
                               .ToListAsync();

                _result.Data = bloodTypeSummary;
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

        public async Task<ResultModel> GetBloods(Guid hospitalId, string bloodType)
        {
            try
            {
                var bloods = await _db.Bloods.Where(bl => bl.HospitalId == hospitalId && bl.BloodType == bloodType).ToListAsync();

                _result.Data = bloods;
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

        public async Task<ResultModel> ExportBloods(Guid requestId, Guid hospitalNewId, List<Guid> bloodIds)
        {
            bloodIds.ForEach(bloodId =>
            {
                var blood = _db.Bloods.FirstOrDefault(bl => bl.Id == bloodId);

                if (blood == null) throw new Exception();

                blood.HospitalId = hospitalNewId;
            });
            var request = _db.RequestBloods.FirstOrDefault(rq => rq.Id == requestId);

            if (request == null) throw new Exception();

            request.Status = Data.Enums.StatusRequestBlood.IsCompleted;

            await _db.SaveChangesAsync();

            _result.IsSuccess = true;
            _result.Message = "Export successful";

            return _result;
        }
    }
}
