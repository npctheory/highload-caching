using Microsoft.AspNetCore.Mvc;
using Api.Authentication;
using Api.Services.Authentication;

namespace Api.Controllers;

[ApiController]
public class AuthenticationController: ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        var authResult = _authenticationService.Register(
            request.FirstName,
            request.SecondName,
            request.Birthdate,
            request.Biography,
            request.City,
            request.Password
        );

        var response = new AuthenticationResponse(authResult.token);
        return Ok(response);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authResult = _authenticationService.Login(
            request.Id,
            request.Password
        );

        var response = new AuthenticationResponse("login_token");
        return Ok(response);
    }
}
