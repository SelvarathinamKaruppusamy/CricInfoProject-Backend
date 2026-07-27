using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Infrastructure;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AngularPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
