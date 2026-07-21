using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Repositories.CompletedModule;

using CricInfo.Application.Interfaces.Services.CompletedModule;
using CricInfo.Application.Interfaces.Services.LiveModule;
using LiveMatchRepository = CricInfo.Infrastructure.Repositories.LiveModule.MatchRepository;
using CompletedMatchRepository = CricInfo.Infrastructure.Repositories.CompletedModule.MatchRepository;
using CricInfo.Application.Services.LiveModule;
using CricInfo.Infrastructure.Repositories.LiveModule;
using CricInfo.Infrastructure.Repositories.CompletedModule;

using Microsoft.Extensions.DependencyInjection;

namespace CricInfo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Live Module
        services.AddScoped<
            CricInfo.Application.Interfaces.Repositories.LiveModule.IMatchRepository,
            LiveMatchRepository>();

        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<IPlayerRepository, PlayerRepository>();

        // Completed Module
        services.AddScoped<
            CricInfo.Application.Interfaces.Repositories.CompletedModule.IMatchesRepository,
            CompletedMatchRepository>();

        services.AddScoped<IBattingRepository, BattingRepository>();
        services.AddScoped<IBowlingRepository, BowlingRepository>();

        // Services
        services.AddScoped<ILiveService, LiveService>();
        services.AddScoped<ICompletedService, CompletedService>();

        return services;
    }
}