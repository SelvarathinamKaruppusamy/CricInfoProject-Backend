using AutoMapper;
using CricInfo.Application.DTOs.Blog;
using CricInfo.Application.Interfaces.Repositories.BlogModule;
using CricInfo.Application.Interfaces.Services.BlogModule;
using CricInfo.Domain.Entities;

namespace CricInfo.Application.Services.BlogModule;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;

    public BlogService(
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        _blogRepository = blogRepository;
        _mapper = mapper;
    }

    public async Task<List<BlogDto>> GetAllBlogsAsync()
    {
        var blogs = await _blogRepository.GetAllAsync();

        return _mapper.Map<List<BlogDto>>(blogs);
    }

    public async Task<BlogDto?> GetBlogByMatchNoAsync(int matchNo)
    {
        var blog = await _blogRepository.GetByMatchNoAsync(matchNo);

        if (blog == null)
            return null;

        return _mapper.Map<BlogDto>(blog);
    }

    public async Task<BlogDto> CreateBlogAsync(CreateBlogDto createBlogDto)
    {
        var blog = _mapper.Map<Blog>(createBlogDto);

        var createdBlog = await _blogRepository.AddAsync(blog);

        return _mapper.Map<BlogDto>(createdBlog);
    }

    public async Task<BlogDto?> UpdateBlogAsync(int matchNo, UpdateBlogDto updateBlogDto)
    {
        var existingBlog = await _blogRepository.GetByMatchNoAsync(matchNo);

        if (existingBlog == null)
            return null;

        _mapper.Map(updateBlogDto, existingBlog);

        var updatedBlog = await _blogRepository.UpdateAsync(existingBlog);

        return _mapper.Map<BlogDto>(updatedBlog);
    }

    public async Task<bool> DeleteBlogAsync(int matchNo)
    {
        var blog = await _blogRepository.GetByMatchNoAsync(matchNo);

        if (blog == null)
            return false;

        await _blogRepository.DeleteAsync(blog);

        return true;
    }
}