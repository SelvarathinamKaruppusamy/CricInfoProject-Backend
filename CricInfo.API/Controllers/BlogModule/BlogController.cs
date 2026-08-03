using CricInfo.Application.DTOs.Blog;
using CricInfo.Application.Interfaces.Services.BlogModule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CricInfo.API.Controllers.BlogModule;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var blogs = await _blogService.GetAllBlogsAsync();
        return Ok(blogs);
    }

   [HttpGet("{matchNo:int}")]
public async Task<IActionResult> GetByMatchNo(int matchNo)
{
    var blog = await _blogService.GetBlogByMatchNoAsync(matchNo);

    if (blog == null)
        return NotFound();

    return Ok(blog);
}

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateBlogDto dto)
    {
        var blog = await _blogService.CreateBlogAsync(dto);

        return CreatedAtAction(
            nameof(GetByMatchNo),
            new { matchNo = blog.MatchId },
            blog);
    }

    [Authorize]
    [HttpPut("{matchNo:int}")]
    public async Task<IActionResult> Update(int matchNo, UpdateBlogDto dto)
    {
        var blog = await _blogService.UpdateBlogAsync(matchNo, dto);

        if (blog == null)
            return NotFound();

        return Ok(blog);
    }

    [Authorize]
    [HttpDelete("{matchNo:int}")]
    public async Task<IActionResult> Delete(int matchNo)
    {
        var deleted = await _blogService.DeleteBlogAsync(matchNo);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}