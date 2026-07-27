using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.LiveModule;

public interface ITeamRepository
{
    Task<List<Team>> GetTeamsByMatchNoAsync(int matchNo);
    Task<Team?> GetTeamByIdAsync(int teamId, int matchNo);

    Task UpdateTeamAsync(Team team);
}
