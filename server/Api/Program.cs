using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Persistence;
using Application.Interfaces;
using Infrastructure.Repositories;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddSingleton<PostgresConnectionFactory>();
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(sp.GetRequiredService<PostgresConnectionFactory>().CreateConnection().ConnectionString));
builder.Services.AddControllers();
var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();