using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;

namespace CricInfo.Infrastructure.Repositories.LiveModule;

public class PlayerRepository : IPlayerRepository
{
    private readonly CricDbContext _context;

    public PlayerRepository(CricDbContext context)
    {
        _context = context;
    }

    public async Task<List<Player>> GetPlayersByMatchNoAsync(int matchNo)
    {
        return await _context.Players
      .Where(x => x.matchNo == matchNo)
      .OrderBy(x => x.TeamId)
      .ThenBy(x => x.playerId)
      .ToListAsync();
    }
}