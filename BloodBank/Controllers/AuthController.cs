using BloodBank.Data.Dtos.Authentication;
using BloodBank.Data.Dtos.Donor;
using BloodBank.Data.Dtos.Hospital;
using BloodBank.Service.Cores;
using BloodBank.Service.Utils.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;
        private readonly IUserService _userService;
        public AuthController(JwtTokenHandler jwtTokenHandler, IUserService userService)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _userService = userService;
        }
        [HttpPost]
        public ActionResult<AuthenticationResponse?> Authenticate([FromBody] AuthenticationRequest request)
        {
            var authenticationResponse = _jwtTokenHandler.GenerateJwtToken(request);
            if (authenticationResponse == null) return Unauthorized();

            return authenticationResponse;
        }
        [HttpPost("donor/registry")]
        public async Task<IActionResult> RegistryDonorAccount(DonorDto request)
        {
            var result = await _userService.RegistryDonorAccount(request);
            if (result.IsSuccess == false) return BadRequest("Registry failed");
            return Ok("Registry account successful");
        }
        [HttpPost("hospital/registry")]
        public async Task<IActionResult> RegistryHospitalAccount(HospitalDto request)
        {
            var result = await _userService.RegistryHospitalAccount(request);
            return Ok("Registry account successful");
        }
    }
}
