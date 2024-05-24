using BloodBank.Data.Dtos;
using BloodBank.Data.Enums;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetRequest(StatusRequestBlood statusSession, int page, int pageSize) 
        {
            _result = await _service.GetRequest(statusSession, page, pageSize);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("hospitals/{hospitalId}")]
        public async Task<IActionResult> GetRequestByHospitalId(Guid hospitalId, int page, int pageSize)
        {
            _result = await _service.GetRequestByHospitalId(hospitalId, page, pageSize);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpPost]
        public async Task<IActionResult>CreateRequest(RequestBloodDto requestDto)
        {
            _result = await _service.CreateRequest(requestDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }

        [HttpPut("requests/{requestId}")]
        public async Task<IActionResult> UpdateRequest(Guid requestId, UpdateRequestBloodDto requestDto)
        {
            _result = await _service.UpdateRequest(requestId, requestDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
    }
}
