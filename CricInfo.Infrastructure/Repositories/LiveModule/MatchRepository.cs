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

    public async Task<List<Match>> GetAllMatchesAsync()
    {
        return await _context.Matches.ToListAsync();
    }

    public async Task<Match?> GetMatchByMatchNoAsync(int matchNo)
    {
        return await _context.Matches
            .FirstOrDefaultAsync(x => x.matchNo == matchNo);
    }

    public async Task<List<Match>> GetLiveMatchesAsync()
    {
        return await _context.Matches
            .Where(x => x.status == "Live")
            .ToListAsync();
    }
}