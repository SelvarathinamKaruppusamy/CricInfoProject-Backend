using CricInfo.Application.Interfaces.Repositories.CompletedModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;

namespace CricInfo.Infrastructure.Repositories.CompletedModule;

public class BowlingRepository : IBowlingRepository
{
    private readonly CricDbContext _context;

    public BowlingRepository(CricDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(List<Bowling> bowlingList)
    {
        await _context.Bowling.AddRangeAsync(bowlingList);
    }
}