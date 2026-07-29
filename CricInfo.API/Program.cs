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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<CricDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultString")));

builder.Services.AddInfrastructure();

builder.Services.AddAutoMapper(typeof(ILiveService).Assembly);


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



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