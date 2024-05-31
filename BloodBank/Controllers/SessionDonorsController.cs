using BloodBank.Data.Dtos.Donor;
using BloodBank.Data.Dtos;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BloodBank.Data.Dtos.SessionDonor;
using BloodBank.Data.Enums;
using Microsoft.AspNetCore.Authorization;

namespace BloodBank.Controllers
{
    [Route("api/sessiondonors")]
    [ApiController]
    public class SessionDonorsController : ControllerBase
    {
        private readonly ISessionDonorService _service;
        private ResultModel _result;
        public SessionDonorsController(ISessionDonorService service)
        {
            _service = service;
            _result = new ResultModel();
        }

        [HttpGet("activities/{activityId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> GetByActivityId(Guid activityId, [FromQuery] PagingModel sessionDto, [FromQuery] StatusSession status)
        {
            _result = await _service.GetByActivityId(activityId, sessionDto, status);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("donors/{donorId}")]
        [Authorize(Roles = "Donor,Hospital,Admin")]
        public async Task<IActionResult> GetByDonorId(Guid donorId)
        {
            _result = await _service.GetByDonorId(donorId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpPost]
        [Authorize(Roles = "Donor,Hospital,Admin")]
        public async Task<IActionResult> Post(SessionDonorDto sessionDto)
        {
            _result = await _service.CreateSession(sessionDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }


        [HttpPut("{sessionId}")]
        [Authorize(Roles = "Donor,Hospital,Admin")]
        public async Task<IActionResult> UpdateSession(Guid sessionId, [FromBody] UpdateSessionDto sessionDto)
        {
            _result = await _service.Update(sessionId, sessionDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpDelete("{sessionId}")]
        [Authorize(Roles = "Donor,Hospital,Admin")]
        public async Task<IActionResult> DeleteSession(Guid sessionId)
        {
            _result = await _service.DeleteById(sessionId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
    }
}
