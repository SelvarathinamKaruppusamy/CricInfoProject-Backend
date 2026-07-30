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
using Microsoft.Extensions.DependencyInjection;
using LiveMatchRepository = CricInfo.Infrastructure.Repositories.LiveModule.MatchRepository;

namespace CricInfo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Live Module
        services.AddScoped<
            CricInfo.Application.Interfaces.Repositories.LiveModule.IMatchRepository,
            LiveMatchRepository>();

      
        
        // Repository
        services.AddScoped<IMatchRepository, MatchRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        // Services
        services.AddScoped<ILiveService, LiveService>();
        services.AddScoped<ICompletedService, CompletedService>();
        // Service
        services.AddScoped<ILiveService, LiveService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IBlogService, BlogService>();

        return services;
    }
}