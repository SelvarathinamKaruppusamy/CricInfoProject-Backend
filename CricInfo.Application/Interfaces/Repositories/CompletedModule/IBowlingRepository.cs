using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.CompletedModule;

public interface IBowlingRepository
{
    Task AddRangeAsync(List<Bowling> bowlingList);
}