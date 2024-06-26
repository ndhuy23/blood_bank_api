﻿using BloodBank.Data.Dtos;
using BloodBank.Service.Cores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Controllers
{
    [Route("api/histories")]
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
        [Authorize(Roles = "Hospital,Admin")]
        public async Task<IActionResult> Post(HistoryDto historyDto)
        {
            _result = await _service.CreateHistory(historyDto);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }
        [HttpGet("donors/{donorId}")]
        [Authorize(Roles = "Donor,Hospital,Admin")]
        public async Task<IActionResult> GetHistoryByDonorId(Guid donorId)
        {
            _result = await _service.GetByDonorId(donorId);
            if (!_result.IsSuccess) return BadRequest(_result);
            return Ok(_result);
        }

    }
}
