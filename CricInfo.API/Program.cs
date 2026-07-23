using CricInfo.Application.Interfaces.Services.AdminLogin;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Application.Interfaces.Services.PointsTableModule;

using CricInfo.Application.Mapping.CompletedModule;

using CricInfo.Application.Services.AdminLoginModule;
using CricInfo.Application.Services.PointsTableModule;

using CricInfo.Infrastructure;
using CricInfo.Infrastructure.presistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<CricDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultString")));

builder.Services.AddInfrastructure();

builder.Services.AddAutoMapper(typeof(ILiveService).Assembly);

builder.Services.AddAutoMapper(
    typeof(CompletedMappingProfile).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// PointStable Services

builder.Services.AddScoped<
    IPointsTableService,
    PointsTableService>();

// Admin Login Service

builder.Services.AddScoped<
    IAuthService,
    AuthService>();
builder.Services.AddScoped<EmailService>();

// JWT Authentication

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!))
            };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();