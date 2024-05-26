using BloodBank.Data.Dtos;
using BloodBank.Data.Enums;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Controllers
{
    [Route("api/requestbloods/")]
    [ApiController]
    public class RequestBloodsController : ControllerBase
    {
        private readonly IRequestBloodService _service;
        private ResultModel _result;
        public RequestBloodsController(IRequestBloodService service)
        {
            _service = service;
            _result = new ResultModel();
            
        }

        [HttpGet]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> GetRequest(StatusRequestBlood statusSession, int page, int pageSize) 
        {
            _result = await _service.GetRequest(statusSession, page, pageSize);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("hospitals/{hospitalId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> GetRequestByHospitalId(Guid hospitalId, int page, int pageSize)
        {
            _result = await _service.GetRequestByHospitalId(hospitalId, page, pageSize);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpPost]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult>CreateRequest(RequestBloodDto requestDto)
        {
            _result = await _service.CreateRequest(requestDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }

        [HttpPut("requests/{requestId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> UpdateRequest(Guid requestId, UpdateRequestBloodDto requestDto)
        {
            _result = await _service.UpdateRequest(requestId, requestDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
    }
}
