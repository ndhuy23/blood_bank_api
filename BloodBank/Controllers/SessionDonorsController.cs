using BloodBank.Data.Dtos.Donor;
using BloodBank.Data.Dtos;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BloodBank.Data.Dtos.SessionDonor;

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
        public async Task<IActionResult> GetByActivityId(Guid activityId, [FromQuery] PagingModel sessionDto)
        {
            _result = await _service.GetByActivityId(activityId, sessionDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("donors/{donorId}")]
        public async Task<IActionResult> GetByDonorId(Guid donorId)
        {
            _result = await _service.GetByDonorId(donorId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpPost]
        public async Task<IActionResult> Post(SessionDonorDto sessionDto)
        {
            _result = await _service.CreateSession(sessionDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }


        [HttpPut("{sessionId}")]
        public async Task<IActionResult> UpdateSession(Guid sessionId, [FromBody] UpdateSessionDto sessionDto)
        {
            _result = await _service.Update(sessionId, sessionDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpDelete("{sessionId}")]
        public async Task<IActionResult> DeleteSession(Guid sessionId)
        {
            _result = await _service.DeleteById(sessionId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
    }
}
