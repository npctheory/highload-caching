using Microsoft.AspNetCore.Mvc;
using Api.DTO;
using Api.Services.Authentication;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request.Id, request.Password);
            var response = new AuthenticationResponse(result.Token);
            return Ok(response);
        }
    }
}
