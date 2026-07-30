using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.LiveModule;

public interface ITeamRepository
{
    Task<List<Team>> GetTeamsByMatchNoAsync(int matchNo);
    Task<Team?> GetTeamByIdAsync(int teamId, int matchNo);

    Task UpdateTeamAsync(Team team);
<<<<<<< HEAD
=======

>>>>>>> ff4532ef6713a1f27084dc990faf472b3483d223
    Task<List<Team>> GetTeamsUntilMatchAsync(int matchNo);
    Task<List<Team>> GetAllTeamsAsync();

}

