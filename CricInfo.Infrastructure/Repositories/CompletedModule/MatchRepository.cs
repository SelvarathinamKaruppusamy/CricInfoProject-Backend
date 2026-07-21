using CricInfo.Application.Interfaces.Repositories.CompletedModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;

namespace CricInfo.Infrastructure.Repositories.CompletedModule
{
    public class MatchRepository : IMatchesRepository
    {
        private readonly CricDbContext _context;

        public MatchRepository(CricDbContext context)
        {
            _context = context;
        }

        public async Task<List<Match>> GetCompletedMatchesAsync()
        {
            return await _context.Matches
                .Where(m => m.status == "COMPLETED")
                .ToListAsync();
        }

        public async Task<Match?> GetMatchByMatchNoAsync(int matchNo)
        {
            return await _context.Matches
                .FirstOrDefaultAsync(m => m.matchNo == matchNo);
        }
    }
}