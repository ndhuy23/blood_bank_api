﻿using BloodBank.Data.Dtos;
using BloodBank.Data.Dtos.Donor;
using BloodBank.Data.Dtos.Hospital;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Controllers
{
    [Route("api/donors")]
    [ApiController]
    public class DonorsController : ControllerBase
    {
        private readonly IDonorService _service;
        private ResultModel _result;
        public DonorsController(IDonorService service)
        {
            _service = service;
            _result = new ResultModel();
        }

        [HttpGet]
        public async Task<IActionResult> GetDonor([FromQuery] PagingModel donorDto)
        {
            _result = await _service.GetDonor(donorDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("{donorId}")]
        public async Task<IActionResult> GetById(Guid donorId)
        {
            _result = await _service.GetById(donorId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            _result.Data = new Test { Price = 2 };
            //if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("tes2t")]
        public async Task<IActionResult> Te2st()
        {
            _result.Data = new Test { Price = null };
            //if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }

        [HttpPut("{donorId}")]
        public async Task<IActionResult> GetDonor(Guid donorId, [FromBody] DonorDto donorDto)
        {
            _result = await _service.Update(donorId, donorDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpDelete("{donorId}")]
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> DeleteDonor(Guid donorId)
        {
            _result = await _service.DeleteById(donorId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
    }
}
