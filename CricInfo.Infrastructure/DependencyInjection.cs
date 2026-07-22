using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Repositories.Upcoming;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Application.Interfaces.Services.Upcoming;
using CricInfo.Application.Services.LiveModule;
using CricInfo.Application.Services.Upcoming;
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


        // Service

        services.AddScoped<ILiveService, LiveService>();
        services.AddScoped<IUpcomingService, UpcomingService>();

        return services;
    }
}