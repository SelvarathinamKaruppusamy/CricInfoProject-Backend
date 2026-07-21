using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Services.CompletedModule;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Application.Services.CompletedModule;
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
        services.AddScoped<IBattingRepository, BattingRepository>();
        services.AddScoped<IBowlingRepository, BowlingRepository>();

        // Service

        services.AddScoped<ILiveService, LiveService>();
        services.AddScoped<ICompletedService, CompletedService>();

        return services;
    }
}