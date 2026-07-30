using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace CricInfo.Infrastructure.Repositories.LiveModule;

public class PlayerRepository : IPlayerRepository
{
    private readonly CricDbContext _context;

    public PlayerRepository(CricDbContext context)
    {
        _context = context;
    }

    public async Task<Player?> GetPlayerByIdAsync(int playerId, int teamId, int matchNo)
    {
        return await _context.Players.FirstOrDefaultAsync(x=>x.playerId==playerId && x.TeamId==teamId && x.matchNo==matchNo);
    }

    public async Task<List<Player>> GetPlayersByMatchNoAsync(int matchNo)
    {
        return await _context.Players
      .Where(x => x.matchNo == matchNo)
      .OrderBy(x => x.TeamId)
      .ThenBy(x => x.playerId)
      .ToListAsync();
    }

    public async Task<List<Player>> GetPlayersByTeamAsync(int teamId, int matchNo)
    {
        return await _context.Players
            .Where(x => x.TeamId == teamId && x.matchNo == matchNo)
            .OrderBy(x => x.playerId)
            .ToListAsync();
    }

    public async Task UpdatePlayerAsync(Player player)
    {
        await _context.SaveChangesAsync();
    }
    public async Task<Player?> GetNextBatterAsync(int teamId, int matchNo)
    {
        return await _context.Players
            .Where(x =>
                x.TeamId == teamId &&
                x.matchNo == matchNo &&
                x.status == "Yet To Play")
            .OrderBy(x => x.playerId)
            .FirstOrDefaultAsync();
    }
    public async Task DeletePlayersByMatchNoAsync(int matchNo)
    {
        var players = await _context.Players
            .Where(x => x.matchNo == matchNo)
            .ToListAsync();

        _context.Players.RemoveRange(players);
    }
}