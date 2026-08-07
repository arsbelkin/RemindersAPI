using Microsoft.EntityFrameworkCore;
using Reminders.API.Extensions;
using Scalar.AspNetCore;
using Reminders.Infrastructure.Contexts;

using Reminders.Infrastructure.Repositories.Interfaces;
using Reminders.Infrastructure.Repositories;

using Reminders.Application.Services.Interfaces;
using Reminders.Application.Services;

using Reminders.Application.Common.Interfaces;
using Reminders.Application.Common;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<RemindersContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<RemindersContext>();
builder.Services.AddTransient<IHasher, BCryptHasherService>();

builder.Services.AddJWTAuthentication(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
