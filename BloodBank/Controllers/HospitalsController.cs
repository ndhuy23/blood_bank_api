using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Hospital;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Controllers
{
    [Route("api/hospitals")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly IHospitalService _service;
        private ResultModel _result;

        public HospitalsController(IHospitalService service)
        {
            _service = service;
            _result = new ResultModel();
        }
        [HttpGet]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> GetHospital([FromQuery] PagingModel hospital)
        {
            _result = await _service.GetHospitals(hospital);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("{hospitalId}")]
        public async Task<IActionResult> GetHospitalById(Guid hospitalId)
        {
            _result = await _service.GetHospitalById(hospitalId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        
        [HttpPut("{hospitalId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> UpdateHospital(Guid hospitalId, [FromBody]HospitalDto hospital)
        {
            _result = await _service.UpdateAsync(hospitalId, hospital);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpDelete("{hospitalId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHospital(Guid hospitalId)
        {
            _result = await _service.DeleteById(hospitalId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }

    }
}
