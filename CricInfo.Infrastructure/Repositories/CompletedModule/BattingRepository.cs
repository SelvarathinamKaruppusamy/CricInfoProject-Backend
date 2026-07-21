using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;

namespace CricInfo.Infrastructure.Repositories.LiveModule
{
    public class BattingRepository : IBattingRepository
    {
        private readonly CricDbContext _context;

        public BattingRepository(CricDbContext context)
        {
            _context = context;
        }

        public async Task<List<Batting>> GetBattingByMatchNoAsync(int matchNo, int teamId)
        {
            return await _context.Batting
                .Where(x => x.MatchNo == matchNo &&
                            x.TeamId == teamId)
                .ToListAsync();
        }
    }
}