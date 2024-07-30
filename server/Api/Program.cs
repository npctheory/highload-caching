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
