using CricInfo.Application.DTOs.Blog;

namespace CricInfo.Application.Interfaces.Services.BlogModule;

public interface IBlogService
{
    Task<List<BlogDto>> GetAllBlogsAsync();

    Task<BlogDto?> GetBlogByMatchNoAsync(int matchNo);

    Task<BlogDto> CreateBlogAsync(CreateBlogDto createBlogDto);

    Task<BlogDto?> UpdateBlogAsync(int matchNo, UpdateBlogDto updateBlogDto);

    Task<bool> DeleteBlogAsync(int matchNo);
}