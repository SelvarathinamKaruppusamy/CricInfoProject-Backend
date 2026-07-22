using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;

namespace CricInfo.Infrastructure.Repositories.LiveModule;

public class TeamRepository : ITeamRepository
{
    private readonly CricDbContext _context;

    public TeamRepository(CricDbContext context)
    {
        _context = context;
    }

    public async Task<List<Team>> GetTeamsByMatchNoAsync(int matchNo)
    {
        return await _context.Teams
       .Where(x => x.matchNo == matchNo)
       .OrderBy(x => x.TeamId)
       .ToListAsync();
    }
}