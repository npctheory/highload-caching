using System.Net.Mime;
using Api.Authentication;
using Api.Services.Authentication;
using Api.Services;
using Api.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
{
    var configuration = builder.Configuration;

    builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    builder.Services.AddSingleton<IJwtTokenGenerator,JwtTokenGenerator>();
    builder.Services.AddSingleton<IDateTimeProvider,DateTimeProvider>();

    var connectionString = $"Host={Environment.GetEnvironmentVariable("PRIMARY_DB_HOST")};" +
                        $"Port={Environment.GetEnvironmentVariable("PRIMARY_DB_PORT")};" +
                        $"Database={Environment.GetEnvironmentVariable("PRIMARY_DB_NAME")};" +
                        $"Username={Environment.GetEnvironmentVariable("PRIMARY_DB_USER")};" +
                        $"Password={Environment.GetEnvironmentVariable("PRIMARY_DB_PASSWORD")}";
                        
    builder.Services.AddScoped<IUsersRepository>(sp => new UsersRepository(connectionString));
    builder.Services.AddScoped<ITokensRepository>(sp => new TokensRepository(connectionString));
    builder.Services.AddScoped<IFriendsRepository>(sp => new FriendsRepository(connectionString));

    builder.Services.AddScoped<IAuthenticationService,AuthenticationService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IFriendsService, FriendsService>();

    builder.Services.AddControllers();


    var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
    var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
}

var app = builder.Build();

{
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
