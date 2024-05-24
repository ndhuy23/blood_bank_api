using AutoMapper;
using BloodBank.Data.DataAccess;
using BloodBank.Data.Dtos;
using BloodBank.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodBank.Service.Cores
{
    public interface IHistoryService
    {
        Task<ResultModel> CreateHistory(HistoryDto historyDto);
        Task<ResultModel> GetByDonorId(Guid donorId);
    }
    public class HistorySerivce : IHistoryService
    {
        private readonly BloodBankContext _db;
        private ResultModel _result;
        private readonly IMapper _mapper;
        public HistorySerivce(BloodBankContext db, IMapper mapper)
        {
            _db = db;
            _result = new ResultModel();
            _mapper = mapper;
        }
        public async Task<ResultModel> CreateHistory(HistoryDto historyDto)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var history = _mapper.Map<History>(historyDto);
                    history.DonationDate = DateTime.Now;

                    await _db.Histories.AddAsync(history);
                    await _db.SaveChangesAsync();

                    transaction.Commit();
                    _result.Data = history;
                    _result.IsSuccess = true;
                    _result.Message = "Create successful";

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

        public async Task<ResultModel> GetByDonorId(Guid donorId)
        {
            try
            {
                var histories = await _db.Histories.Where(h => h.DonorId == donorId).ToListAsync();
                _result.Data = histories;
                _result.IsSuccess = true;
                _result.Message = "Create successful";
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
