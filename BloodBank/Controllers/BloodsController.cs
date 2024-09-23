using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Donor;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> Create(BloodDto bloodDto)
        {
            _result = await _service.CreateBlood(bloodDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> GetBlood([FromQuery] PagingModel donorDto)
        {
            _result = await _service.GetBlood(donorDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("hospitals/{hospitalId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> GetBloodByHospitalId(Guid hospitalId)
        {
            _result = await _service.GetBloodByHospitalId(hospitalId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("{bloodId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> GetById(Guid bloodId)
        {
            _result = await _service.GetById(bloodId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpPut("{bloodId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> GetDonor(Guid donorId, [FromBody] BloodDto bloodId)
        {
            _result = await _service.Update(donorId, bloodId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpDelete("{bloodId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> DeleteDonor(Guid bloodId)
        {
            _result = await _service.DeleteById(bloodId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("/hospitals/{hospitalId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> GetBloodByHospitalIdAndBloodType(Guid hospitalId, string bloodType)
        {
            _result = await _service.GetBloods(hospitalId, bloodType);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpPut("export/{requestId}/{hospitalNewId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> ExportBloods(Guid requestId,Guid hospitalNewId, [FromBody] List<Guid> bloodIds)
        {
            _result = await _service.ExportBloods(requestId, hospitalNewId, bloodIds);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
    }
}
