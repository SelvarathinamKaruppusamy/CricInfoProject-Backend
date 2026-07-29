using CricInfo.Application.Interfaces.Repositories.CompletedModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;

namespace CricInfo.Infrastructure.Repositories.CompletedModule
{
    public class CompletedRepository : ICompletedRepository
    {
        private readonly CricDbContext _context;

        public CompletedRepository(CricDbContext context)
        {
            _context = context;
        }

        // ============================
        // COMPLETED MATCHES
        // ============================

        public async Task<List<Match>> GetCompletedMatchesAsync()
        {
            return await _context.Matches
                .AsNoTracking()
                .Where(m => m.status == "COMPLETED")
                .OrderBy(m => m.matchNo)
                .ToListAsync();
        }

        public async Task<Match?> GetCompletedMatchAsync(int matchNo)
        {
            return await _context.Matches
                .AsNoTracking()
                .FirstOrDefaultAsync(m =>
                    m.matchNo == matchNo &&
                    m.status == "COMPLETED");
        }

        // ============================
        // BULK METHODS
        // ============================

        public async Task<List<Team>> GetTeamsByMatchNosAsync(List<int> matchNos)
        {
            return await _context.Teams
                .AsNoTracking()
                .Where(t => matchNos.Contains(t.matchNo))
                .ToListAsync();
        }

        public async Task<List<Batting>> GetBattingByMatchNosAsync(List<int> matchNos)
        {
            return await _context.Batting
                .AsNoTracking()
                .Where(b => matchNos.Contains(b.MatchNo))
                .ToListAsync();
        }

        public async Task<List<Bowling>> GetBowlingByMatchNosAsync(List<int> matchNos)
        {
            return await _context.Bowling
                .AsNoTracking()
                .Where(b => matchNos.Contains(b.MatchNo))
                .ToListAsync();
        }

        // ============================
        // SINGLE MATCH METHODS
        // ============================

        public async Task<List<Team>> GetTeamsByMatchNoAsync(int matchNo)
        {
            return await _context.Teams
                .AsNoTracking()
                .Where(t => t.matchNo == matchNo)
                .ToListAsync();
        }

        public async Task<List<Batting>> GetBattingByMatchNoAsync(int matchNo)
        {
            return await _context.Batting
                .AsNoTracking()
                .Where(b => b.MatchNo == matchNo)
                .ToListAsync();
        }

        public async Task<List<Bowling>> GetBowlingByMatchNoAsync(int matchNo)
        {
            return await _context.Bowling
                .AsNoTracking()
                .Where(b => b.MatchNo == matchNo)
                .ToListAsync();
        }
    }
}