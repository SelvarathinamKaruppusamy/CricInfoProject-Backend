using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.CompletedModule
{
    public interface ICompletedRepository
    {
        Task<List<Match>> GetCompletedMatchesAsync();

        Task<Match?> GetCompletedMatchAsync(int matchNo);

        Task<List<Team>> GetTeamsByMatchNosAsync(List<int> matchNos);

        Task<List<Batting>> GetBattingByMatchNosAsync(List<int> matchNos);

        Task<List<Bowling>> GetBowlingByMatchNosAsync(List<int> matchNos);

        Task<List<Team>> GetTeamsByMatchNoAsync(int matchNo);

        Task<List<Batting>> GetBattingByMatchNoAsync(int matchNo);

        Task<List<Bowling>> GetBowlingByMatchNoAsync(int matchNo);
    }
}