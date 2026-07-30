using CricInfo.API.Filters;
using CricInfo.API.Middleware;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Infrastructure;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ActionLoggingFilter>();
builder.Services.AddControllers(options=> { options.Filters.Add<ActionLoggingFilter>();});
builder.Services.AddOpenApi();
builder.Services.AddDbContext<CricDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultString")));
builder.Services.AddInfrastructure();
builder.Services.AddAutoMapper(typeof(ILiveService).Assembly);
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
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AngularPolicy");
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
