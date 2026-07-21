using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.LiveModule
{
    public interface IBowlingRepository
    {
        Task<List<Bowling>> GetBowlingByMatchNoAsync(int matchNo, int teamId);
    }
}