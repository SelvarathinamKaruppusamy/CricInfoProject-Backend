using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.LiveModule;

public interface IMatchRepository
{

    Task<Match?> GetMatchByMatchNoAsync(int matchNo);

    Task<List<Match>> GetLiveMatchesAsync();
    Task UpdateMatchAsync(Match match);
    Task<List<Match>> GetUpcomingMatchesAsync();
    Task<Match?> GetCurrentLiveMatchAsync();

}
   
