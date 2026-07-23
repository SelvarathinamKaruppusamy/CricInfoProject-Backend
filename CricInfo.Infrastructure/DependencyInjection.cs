using CricInfo.Application.Interfaces.Repositories.BlogModule;
using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Services.BlogModule;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Application.Services.BlogModule;
using CricInfo.Application.Services.LiveModule;
using CricInfo.Infrastructure.Repositories.BlogModule;
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
        services.AddScoped<IBlogRepository, BlogRepository>();

        // Service

        services.AddScoped<ILiveService, LiveService>();
        services.AddScoped<IBlogService, BlogService>();

        return services;
    }
}