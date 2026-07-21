using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.LiveModule
{
    public interface IBattingRepository
    {
        Task<List<Batting>> GetBattingByMatchNoAsync(int matchNo, int teamId);
    }
}