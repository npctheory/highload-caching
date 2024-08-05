using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Common;
using Application.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Providers;
using Infrastructure.Generators;
using Npgsql;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Mapping;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);
{
    var configuration = builder.Configuration;
    builder.Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
    builder.Services.AddSingleton<IJwtTokenGenerator,JwtTokenGenerator>();
    builder.Services.AddSingleton<IDateTimeProvider,DateTimeProvider>();
    builder.Services.AddSingleton<PostgresConnectionFactory>();

    builder.Services.AddAutoMapper(typeof(MappingProfile));

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    var connectionString = (new PostgresConnectionFactory()).CreateConnection().ConnectionString;

    builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(connectionString));
    builder.Services.AddScoped<ITokenRepository>(sp => new TokenRepository(connectionString));

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
    app.MapControllers();
    app.Run();
}