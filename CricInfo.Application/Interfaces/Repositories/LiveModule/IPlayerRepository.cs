using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.LiveModule;

public interface IPlayerRepository
{
    Task<List<Player>> GetPlayersByMatchNoAsync(int matchNo);
}