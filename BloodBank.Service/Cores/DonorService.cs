using AutoMapper;
using BloodBank.Data.DataAccess;
using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Donor;
using BloodBank.Data.Dtos.Hospital;
using BloodBank.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Cores
{
    public interface IDonorService
    {
        Task<ResultModel> DeleteById(Guid donorId);
        Task<ResultModel> GetById(Guid donorId);
        Task<ResultModel> GetDonor(PagingModel donorDto);
        Task<ResultModel> Update(Guid donorId, DonorDto donorDto);
        
    }
    public class DonorService : IDonorService
    {
        private readonly BloodBankContext _db;
        private ResultModel _result;
        private readonly IMapper _mapper;
        public DonorService(BloodBankContext db, IMapper mapper)
        {
            _db = db;
            _result = new ResultModel();
            _mapper = mapper;
        }
        

        public async Task<ResultModel> DeleteById(Guid donorId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _result = await GetById(donorId);

                    _db.Donors.Remove((Donor)_result.Data);
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    _result.IsSuccess = true;
                    _result.Message = "Delete successful";

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _result.IsSuccess = true;
                    _result.Message = ex.Message;
                }
            }
            return _result;
        }

        public async Task<ResultModel> GetById(Guid donorId)
        {
            try
            {
                var donor = await _db.Donors.FindAsync(donorId);
                if (donor == null) throw new Exception("Donor is not exist");
                _result.Data = donor;
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

        public async Task<ResultModel> GetDonor(PagingModel paging)
        {
            try
            {
                paging.Data = _db.Donors.Skip((paging.Page - 1) * paging.PageSize)
                .Take(paging.PageSize)
                .ToListAsync();

                paging.TotalCount = await _db.Donors.CountAsync();

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

        

        public async Task<ResultModel> Update(Guid donorId, DonorDto donorDto)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _result = await GetById(donorId);

                    donorDto.Password = BCrypt.Net.BCrypt.HashPassword(donorDto.Password);
                    _mapper.Map(donorDto, _result.Data);

                    await _db.SaveChangesAsync();
                    transaction.Commit();
                    _result.IsSuccess = true;
                    _result.Message = "Update successful";

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _result.IsSuccess = true;
                    _result.Message = ex.Message;
                }
            }

            return _result;
        }
    }
}
