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
        private readonly IAccountService _accountService;
        public AuthController(JwtTokenHandler jwtTokenHandler, IAccountService accountService)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _accountService = accountService;
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
            await _accountService.RegistryDonorAccount(request);
            return Ok("Registry account successful");
        }
        [HttpPost("hospital/registry")]
        public async Task<IActionResult> RegistryHospitalAccount(HospitalDto request)
        {
            await _accountService.RegistryHospitalAccount(request);
            return Ok("Registry account successful");
        }
    }
}
