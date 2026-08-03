using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.LiveModule;

public interface ITeamRepository
{
    Task<List<Team>> GetTeamsByMatchNoAsync(int matchNo);
    Task<Team?> GetTeamByIdAsync(int teamId, int matchNo);

    Task UpdateTeamAsync(Team team);
    Task<List<Team>> GetTeamsUntilMatchAsync(int matchNo);
    Task<List<Team>> GetAllTeamsAsync();

}

