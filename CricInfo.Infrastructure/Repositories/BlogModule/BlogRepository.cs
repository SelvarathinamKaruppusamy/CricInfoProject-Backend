using CricInfo.Application.Interfaces.Repositories.BlogModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;

namespace CricInfo.Infrastructure.Repositories.BlogModule;

public class BlogRepository : IBlogRepository
{
    private readonly CricDbContext _context;

    public BlogRepository(CricDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Blog>> GetAllAsync()
    {
        return await _context.Blogs
            .AsNoTracking()
            .OrderByDescending(b => b.PublishedDate)
            .ToListAsync();
    }

    public async Task<Blog?> GetByMatchNoAsync(int matchNo)
    {
        return await _context.Blogs
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.MatchNo == matchNo);
    }    

    public async Task<Blog> AddAsync(Blog blog)
    {
        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();

        return blog;
    }

    public async Task<Blog> UpdateAsync(Blog blog)
    {
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync();

        return blog;
    }

    public async Task DeleteAsync(Blog blog)
    {
        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Blogs.AnyAsync(b => b.Id == id);
    }
}