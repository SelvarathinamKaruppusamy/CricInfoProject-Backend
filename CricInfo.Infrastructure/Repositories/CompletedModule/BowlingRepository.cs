using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;

namespace CricInfo.Infrastructure.Repositories.LiveModule
{
    public class BowlingRepository : IBowlingRepository
    {
        private readonly CricDbContext _context;

        public BowlingRepository(CricDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bowling>> GetBowlingByMatchNoAsync(int matchNo, int teamId)
        {
            return await _context.Bowling
                .Where(x => x.MatchNo == matchNo &&
                            x.TeamId == teamId)
                .ToListAsync();
        }
    }
}