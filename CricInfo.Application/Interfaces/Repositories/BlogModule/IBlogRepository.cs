using CricInfo.Domain.Entities;

namespace CricInfo.Application.Interfaces.Repositories.BlogModule;

public interface IBlogRepository
{
    Task<IEnumerable<Blog>> GetAllAsync();

    Task<Blog?> GetByMatchNoAsync(int matchNo);

    Task<Blog> AddAsync(Blog blog);

    Task<Blog> UpdateAsync(Blog blog);

    Task DeleteAsync(Blog blog);

    Task<bool> ExistsAsync(int id);
}