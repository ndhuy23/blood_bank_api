using AutoMapper;
using BloodBank.Data.DataAccess;
using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Donor;
using BloodBank.Data.Entities;
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

                    await _db.Bloods.AddAsync(blood);
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

        public Task<ResultModel> GetById(Guid donorId)
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
    }
}
