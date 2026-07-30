using CricInfo.Application.Interfaces.Repositories.CompletedModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;

namespace CricInfo.Infrastructure.Repositories.CompletedModule;

public class BattingRepository : IBattingRepository
{
    private readonly CricDbContext _context;

    public BattingRepository(CricDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(List<Batting> battingList)
    {
        await _context.Batting.AddRangeAsync(battingList);
    }
}