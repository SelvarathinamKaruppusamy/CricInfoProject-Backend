using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.CompletedModule;

public interface IBattingRepository
{
    Task AddRangeAsync(List<Batting> battingList);
}