using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.LiveModule;

public interface IPlayerRepository
{
    Task<List<Player>> GetPlayersByMatchNoAsync(int matchNo);
    Task<List<Player>> GetPlayersByTeamAsync(int teamId, int matchNo);
    Task<Player?> GetPlayerByIdAsync(int playerId, int teamId, int matchNo);
    Task UpdatePlayerAsync(Player player);
    Task<Player?> GetNextBatterAsync(int teamId, int matchNo);
    Task DeletePlayersByMatchNoAsync(int matchNo);
}