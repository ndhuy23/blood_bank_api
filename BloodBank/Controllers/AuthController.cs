using BloodBank.Data.Dtos.Authentication;
using BloodBank.Service.Utils.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;
        public AuthController(JwtTokenHandler jwtTokenHandler)
        {
            _jwtTokenHandler = jwtTokenHandler;
        }
        [HttpPost]
        public ActionResult<AuthenticationResponse?> Authenticate([FromBody] AuthenticationRequest request)
        {
            var authenticationResponse = _jwtTokenHandler.GenerateJwtToken(request);
            if (authenticationResponse == null) return Unauthorized();

            return authenticationResponse;
        }
    }
}
