﻿using AutoMapper;
using BloodBank.Data.DataAccess;
using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Hospital;
using BloodBank.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Cores
{

    public interface IHospitalService
    {
        Task<ResultModel> DeleteById(Guid hospitalId);
        Task<ResultModel> GetHospitals(PagingModel hospital);
        Task<ResultModel> UpdateAsync(Guid hospitalId, HospitalDto hospital);
        Task<ResultModel> GetHospitalById(Guid hospitalId);
    }
    public class HospitalService : IHospitalService
    {
        private readonly BloodBankContext _db;
        private ResultModel _result; 
        private readonly IMapper _mapper;
        public HospitalService(BloodBankContext db, IMapper mapper)
        {
            _db = db;
            _result = new ResultModel();
            _mapper = mapper;
        }
        
        public async Task<ResultModel> GetHospitalById(Guid hospitalId)
        {
            try
            {
                var hospital = _db.Users.FirstOrDefault(u => u.Id == hospitalId && u.Role == Role.Hospital);
                if (hospital == null) throw new Exception("Hospital is not exist");
                _result.Data = hospital;
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
        
        public async Task<ResultModel> GetHospitals(PagingModel paging)
        {
            try
            {
                paging.Data = await _db.Users.Where(u => u.Role == Role.Hospital).Skip((paging.Page - 1) * paging.PageSize)
                                                .Take(paging.PageSize)
                                                .ToListAsync();

                paging.TotalCount = await _db.Users.CountAsync();
                
                _result.Data = paging;
                _result.IsSuccess = true;
                _result.Message = "";
            }
            catch(Exception ex)
            {
                _result.IsSuccess = false;
                _result.Message = ex.Message;
            }
            return _result;
        }

        public async Task<ResultModel> UpdateAsync(Guid hospitalId, HospitalDto hospitalDto)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _result = await GetHospitalById(hospitalId);
                    
                    hospitalDto.Password = BCrypt.Net.BCrypt.HashPassword(hospitalDto.Password);
                    _mapper.Map(hospitalDto, _result.Data);

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                    _result.IsSuccess = true;
                    _result.Message = "Update successful";

                }catch(Exception ex)
                {
                    _result.IsSuccess = true;
                    _result.Message = ex.Message;
                }
            }

            return _result;
        }

        public async Task<ResultModel> DeleteById(Guid hospitalId)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _result = await GetHospitalById(hospitalId);

                    _db.Users.Remove((User)_result.Data);
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
    }
}
