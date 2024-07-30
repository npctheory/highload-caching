# highload-caching
```bash
dotnet new sln -o HighloadSocial
mv HighloadSocial server
dotnet new webapi -o ./server/Api
dotnet sln ./server/HighloadSocial.sln add server/Api/Api.csproj

dotnet add ./server/Api/ package Microsoft.Extensions.DependencyInjection.Abstractions
dotnet add ./server/Api/ package System.IdentityModel.Tokens.Jwt
dotnet add ./server/Api/ package Microsoft.Extensions.Options.ConfigurationExtensions

mkdir -p server/Api/Authentication
mkdir -p server/Api/Controllers
mkdir -p server/Api/Services/Authentication
mkdir -p server/Api/Common/Interfaces/Authentication
mkdir -p server/Api/Common/Interfaces/Services
```
**IJwtTokenGenerator.cs**
```bash
cat > server/Api/Common/Interfaces/Authentication/IJwtTokenGenerator.cs << 'EOF'
namespace Api.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(string user_id,string first_name,string second_name);
}
EOF
```
**JwtTokenGenerator.cs**
```bash
cat > server/Api/Authentication/JwtTokenGenerator.cs << 'EOF'
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Api.Common.Interfaces.Authentication;
using Api.Common.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Api.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(string user_id, string first_name, string second_name)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user_id),
            new Claim(JwtRegisteredClaimNames.GivenName, first_name),
            new Claim(JwtRegisteredClaimNames.FamilyName, second_name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes),
            claims:claims,
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

}
EOF
```
**JwtSettings.cs**
```bash
cat > server/Api/Authentication/JwtSettings.cs << 'EOF'
namespace Api.Authentication;

public class JwtSettings
{
    public string Secret {get; init;} = null!;
    public int ExpirationTimeInMinutes {get;init;}
    public string Issuer {get;init;} = null!;
    public string Audience{get;init;} = null!;
}
EOF
```

**IDateTimeProvider.cs**
```bash
cat > server/Api/Common/Interfaces/Services/IDateTimeProvider.cs << 'EOF'
namespace Api.Common.Interfaces.Services;

public interface IDateTimeProvider
{
    DateTime UtcNow {get;}
}
EOF
```
**DateTimeProvider.cs**
```bash
cat > server/Api/Services/DateTimeProvider.cs << 'EOF'
using Api.Common.Interfaces.Services;

namespace Api.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
EOF
```

**LoginRequest.cs**  
```bash
cat > server/Api/Authentication/LoginRequest.cs << 'EOF'
using System.Text.Json.Serialization;

namespace Api.Authentication;

public record LoginRequest(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("password")] string Password
);
EOF
```
**RegisterRequest.cs**
```bash
cat > server/Api/Authentication/RegisterRequest.cs << 'EOF'
using System.Text.Json.Serialization;

namespace Api.Authentication;

public record RegisterRequest(
    [property: JsonPropertyName("first_name")] string FirstName,
    [property: JsonPropertyName("second_name")] string SecondName,
    [property: JsonPropertyName("birthdate")] string Birthdate,
    [property: JsonPropertyName("biography")] string Biography,
    [property: JsonPropertyName("city")] string City,
    [property: JsonPropertyName("password")] string Password
);
EOF
```
**AuthenticationResponse.cs**
```bash
cat > server/Api/Authentication/AuthenticationResponse.cs << 'EOF'
namespace Api.Authentication;

public record AuthenticationResponse(string token);
EOF
```
**AuthenticationController.cs**
```bash
cat > server/Api/Controllers/AuthenticationController.cs << 'EOF'
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
EOF
```
**AuthenticationResult.cs**  
```bash
cat > server/Api/Services/Authentication/AuthenticationResult.cs << 'EOF'
namespace Api.Services.Authentication;

public record AuthenticationResult(
    string id,
    string first_name,
    string second_name,
    string birthdate,
    string biography,
    string city,
    string token
);
EOF
```
**IAuthenticationService.cs**  
```bash
cat > server/Api/Services/Authentication/IAuthenticationService.cs << 'EOF'
namespace Api.Services.Authentication;

public interface IAuthenticationService
{
    AuthenticationResult Login(string id, string password);
    AuthenticationResult Register(string first_name, string second_name, string birthdate, string biography, string city, string password);
}
EOF
```
**AuthenticationService.cs**  
```bash
cat > server/Api/Services/Authentication/AuthenticationService.cs << 'EOF'
using Api.Common.Interfaces.Authentication;

namespace Api.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{   
    private readonly IJwtTokenGenerator _jwtTokenGerator;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGerator = jwtTokenGenerator;
    }


    public AuthenticationResult Login(string id, string password)
    {
        var authResult = new AuthenticationResult(
            id: "123",
            first_name: "John",
            second_name: "Doe",
            birthdate: "2000-01-01",
            biography: "Software Developer",
            city: "New York",
            token: "abcdefg"
        );

        return authResult;
    }

    public AuthenticationResult Register(string first_name, string second_name, string birthdate, string biography, string city, string password)
    {
        string user_id = "randomword001";

        var token = _jwtTokenGerator.GenerateToken(user_id, first_name, second_name);

        var authResult = new AuthenticationResult(
            id: "123",
            first_name: "John",
            second_name: "Doe",
            birthdate: "2000-01-01",
            biography: "Software Developer",
            city: "New York",
            token: token
        );

        return authResult;
    }
}
EOF
```
**Program.cs**
```bash
cat > server/Api/Program.cs << 'EOF'
using System.Net.Mime;
using Api.Authentication;
using Api.Common.Interfaces.Authentication;
using Api.Common.Interfaces.Services;
using Api.Services.Authentication;
using Api.Services;


var builder = WebApplication.CreateBuilder(args);
{
    var configuration = builder.Configuration;

    builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    builder.Services.AddSingleton<IJwtTokenGenerator,JwtTokenGenerator>();
    builder.Services.AddSingleton<IDateTimeProvider,DateTimeProvider>();
    builder.Services.AddScoped<IAuthenticationService,AuthenticationService>();
    builder.Services.AddControllers();
}

var app = builder.Build();

{
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}
EOF
```