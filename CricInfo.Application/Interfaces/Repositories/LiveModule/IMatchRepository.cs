using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.LiveModule;

public interface IMatchRepository
{
    Task<List<Match>> GetAllMatchesAsync();

    Task<Match?> GetMatchByMatchNoAsync(int matchNo);

    Task<List<Match>> GetLiveMatchesAsync();
}