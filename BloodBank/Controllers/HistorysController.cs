using BloodBank.Data.Dtos;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorysController : ControllerBase
    {
        private readonly IHistoryService _service;
        private ResultModel _result;

        public HistorysController(IHistoryService service)
        {
            _service = service;
            _result = new ResultModel();
        }
        [HttpPost]
        public async Task<IActionResult> Post(HistoryDto historyDto)
        {
            _result = await _service.CreateHistory(historyDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("donors/{donorId}")]
        public async Task<IActionResult> GetHistoryByDonorId(Guid donorId)
        {
            _result = await _service.GetByDonorId(donorId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }

    }
}
