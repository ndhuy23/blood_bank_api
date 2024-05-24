using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Donor;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Controllers
{
    [Route("api/bloods")]
    [ApiController]
    public class BloodsController : ControllerBase
    {
        private readonly IBloodService _service;
        private ResultModel _result;

        public BloodsController(IBloodService service)
        {
            _service = service;
            _result = new ResultModel();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BloodDto bloodDto)
        {
            _result = await _service.CreateBlood(bloodDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet]
        public async Task<IActionResult> GetBlood([FromQuery] PagingModel donorDto)
        {
            _result = await _service.GetBlood(donorDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("{bloodId}")]
        public async Task<IActionResult> GetById(Guid bloodId)
        {
            _result = await _service.GetById(bloodId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpPut("{bloodId}")]
        public async Task<IActionResult> GetDonor(Guid donorId, [FromBody] BloodDto bloodId)
        {
            _result = await _service.Update(donorId, bloodId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpDelete("{bloodId}")]
        public async Task<IActionResult> DeleteDonor(Guid bloodId)
        {
            _result = await _service.DeleteById(bloodId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }

    }
}
