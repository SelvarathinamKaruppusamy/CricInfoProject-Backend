using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.LiveModule;

public interface ITeamRepository
{
    Task<List<Team>> GetTeamsByMatchNoAsync(int matchNo);
    Task<List<Team>> GetTeamsUntilMatchAsync(int matchNo);
    Task<List<Team>> GetAllTeamsAsync();

}