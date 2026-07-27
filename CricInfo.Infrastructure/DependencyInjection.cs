using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Application.Services.LiveModule;
using CricInfo.Infrastructure.Repositories.LiveModule;
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

        // Service
        
        services.AddScoped<ILiveService, LiveService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}