using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Activity;
using BloodBank.Data.Enums;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Controllers
{
    [Route("api/activities")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivityService _service;
        private ResultModel _result;

        public ActivitiesController(IActivityService service)
        {
            _service = service;
            _result = new ResultModel();
        }
        [HttpGet]
        public async Task<IActionResult> GetByCursor(Guid? hospitalId ,DateTime? cursor, DateTimeOffset? from, DateTimeOffset? to, int pageSize, StatusActivity? status)
        {
            _result = await _service.GetActivityByCursor(hospitalId, cursor, from, to, pageSize, status);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("hospitals/{hospitalId}")]
        public async Task<IActionResult> GetByHospitalId(Guid hospitalId, StatusActivity? status)
        {
            _result = await _service.GetActivity(hospitalId, status);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("{activityId}")]
        public async Task<IActionResult> GetById(Guid activityId)
        {
            _result = await _service.GetActivityById(activityId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpPost]
        public async Task<IActionResult> Post(ActivityDto activity)
        {
            _result = await _service.AddActivity(activity);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);

        }
        [HttpPut("{activityId}")] 
        public async Task<IActionResult> Put(Guid activityId, [FromBody] ActivityDto activity)
        {
            _result = await _service.Update(activityId, activity);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpDelete("{activityId}")]
        public async Task<IActionResult> Delete(Guid activityId)
        {
            _result = await _service.Delete(activityId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }

    }
}
