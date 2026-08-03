using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Repositories.Upcoming;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Application.Interfaces.Services.Upcoming;
using CricInfo.Application.Services.LiveModule;
using CricInfo.Application.Services.Upcoming;
using CricInfo.Application.Interfaces.Repositories.BlogModule;
using CricInfo.Application.Interfaces.Repositories.CompletedModule;
using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Services.BlogModule;
using CricInfo.Application.Interfaces.Services.CompletedModule;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Application.Services.BlogModule;
using CricInfo.Application.Services.CompletedModule;
using CricInfo.Application.Services.LiveModule;
using CricInfo.Infrastructure.Repositories.BlogModule;
using CricInfo.Infrastructure.Repositories.CompletedModule;
using CricInfo.Infrastructure.Repositories.LiveModule;
using CricInfo.Infrastructure.Repositories.UpcomingModule;
using Microsoft.Extensions.DependencyInjection;


namespace CricInfo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {

      
        
        // Repository
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IUpcomingRepository, UpcomingRepository>();

        services.AddScoped<IBlogRepository, BlogRepository>();

        // Completed Module
        services.AddScoped<ICompletedRepository, CompletedRepository>();
        services.AddScoped<IBattingRepository, BattingRepository>();
        services.AddScoped<IBowlingRepository, BowlingRepository>();

        // Services
        services.AddScoped<ILiveService, LiveService>();
        services.AddScoped<ICompletedService, CompletedService>();
        // Service
        services.AddScoped<ILiveService, LiveService>();
        services.AddScoped<IUpcomingService, UpcomingService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBlogService, BlogService>();

        return services;
    }
}