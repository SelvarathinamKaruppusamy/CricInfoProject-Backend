using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;

namespace CricInfo.Infrastructure.Repositories.LiveModule;

public class MatchRepository : IMatchRepository
{
    private readonly CricDbContext _context;

    public MatchRepository(CricDbContext context)
    {
        _context = context;
    }
    public async Task<Match?> GetMatchByMatchNoAsync(int matchNo)
    {
        var allMatches = await _context.Matches.ToListAsync();

        return await _context.Matches
            .FirstOrDefaultAsync(x => x.matchNo == matchNo);
    }

    public async Task<List<Match>> GetLiveMatchesAsync()
    {
        return await _context.Matches
            .Where(x => x.status == "LIVE")
            .ToListAsync();
    }

    public async Task UpdateMatchAsync(Match match)
    {
        
        await _context.SaveChangesAsync();
    }
    public async Task<List<Match>> GetUpcomingMatchesAsync()
    {
        return await _context.Matches
            .Where(m => m.status == "UPCOMING")
            .OrderBy(m => m.matchNo)
            .ToListAsync();
    }
    public async Task<Match?> GetCurrentLiveMatchAsync()
    {
        return await _context.Matches
            .FirstOrDefaultAsync(x => x.status == "LIVE");
    }
}