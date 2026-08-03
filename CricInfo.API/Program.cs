using CricInfo.API.Filters;
using CricInfo.API.Middleware;
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
using Serilog;

using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Add services
// ----------------------

builder.Services.AddScoped<ActionLoggingFilter>();
builder.Services.AddControllers(options=> { options.Filters.Add<ActionLoggingFilter>();});
builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<CricDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultString")));

builder.Services.AddInfrastructure();

// AutoMapper
builder.Services.AddAutoMapper(typeof(ILiveService).Assembly);
builder.Services.AddAutoMapper(typeof(CompletedMappingProfile).Assembly);

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

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
// ----------------------
// Points Table Services
// ----------------------

builder.Services.AddScoped<IPointsTableService, PointsTableService>();

// ----------------------
// Admin Login Services
// ----------------------

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<EmailService>();

// ----------------------
// Quiz Services
// ----------------------

builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IQuizService, QuizService>();

// ----------------------
// JWT Authentication
// ----------------------

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// ----------------------
// Build Application
// ----------------------

var app = builder.Build();

// ----------------------
// Configure Middleware
// ----------------------

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AngularPolicy");
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("AngularPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();