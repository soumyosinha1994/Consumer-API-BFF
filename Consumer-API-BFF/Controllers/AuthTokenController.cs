using Consumer_API_BFF.IServices;
using Consumer_API_BFF.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Consumer_API_BFF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthTokenController : Controller
    {
        private readonly IGetAuthTokenService _getAuthTokenService;
        public AuthTokenController(IGetAuthTokenService getAuthTokenService)
        {
            _getAuthTokenService = getAuthTokenService;
        }
        [HttpPost("GetAuthToken")]
        public async Task<IActionResult> Login([FromBody] AuthenticationModel authenticationModel, CancellationToken cancellationToken)
        {
            var token = await _getAuthTokenService.GetAccessToken(authenticationModel, cancellationToken);
            return Ok(new { AuthToken = token });
        }
    }
}
