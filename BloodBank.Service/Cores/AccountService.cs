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
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Cores
{
    public interface IAccountService
    {
        Task<ResultModel> RegistryDonorAccount(DonorDto request);
        Task<ResultModel> RegistryHospitalAccount(HospitalDto request);
    }
    public class AccountService : IAccountService
    {
        private readonly BloodBankContext _db;
        private ResultModel _result;
        private readonly IMapper _mapper;

        public AccountService(BloodBankContext db, IMapper mapper)
        {
            _db = db;
            _result = new ResultModel();
            _mapper = mapper;
        }

        public async Task<ResultModel> RegistryDonorAccount(DonorDto request)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //Check exist
                    var isExisted = await IsAccountExistsAsync(request.Username);
                    if (isExisted) throw new Exception("Username is existed");

                    request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    var account = _mapper.Map<Account>(request);
                    _db.Accounts.Add(account);
                    _db.Donors.Add(new Donor
                    {
                        AccountId = account.Id,
                        Avarta = request.Avarta,
                        Phone = request.Phone
                    });
                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();

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
        public async Task<ResultModel> RegistryHospitalAccount(HospitalDto request)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //Check exist
                    var isExisted = await IsAccountExistsAsync(request.Username);
                    if (isExisted) throw new Exception("Username is existed");

                    request.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    var account = _mapper.Map<Account>(request);
                    _db.Accounts.Add(account);
                    _db.Hospitals.Add(new Hospital
                    {
                        AccountId = account.Id,
                        Avatar = request.Avatar,
                        Address = request.Address
                    });
                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();

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
        public async Task<bool> IsAccountExistsAsync(string username)
        {
            try
            {
                return await _db.Accounts.FirstOrDefaultAsync(d => d.Username == username) != null;
            }
            catch (Exception ex)
            {
                return true;
            }
        }
    }
}
