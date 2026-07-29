using CricInfo.Infrastructure.presistence;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class UnitOfWork : IUnitOfWork
{
    private readonly CricDbContext _context;

    public UnitOfWork(CricDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}