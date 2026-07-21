using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.CompletedModule

{
    public interface IMatchesRepository
    {
        Task<Match?> GetMatchByMatchNoAsync(int matchNo);

        // ADD THIS
        Task<List<Match>> GetCompletedMatchesAsync();
    }
}