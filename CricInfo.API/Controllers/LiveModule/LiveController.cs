using CricInfo.Application.Interfaces.Services.LiveModule;
using Microsoft.AspNetCore.Mvc;

namespace CricInfo.API.Controllers.LiveModule;

[ApiController]
[Route("api/[controller]")]
public class LiveController : ControllerBase
{
    private readonly ILiveService _liveService;

    public LiveController(ILiveService liveService)
    {
        _liveService = liveService;
    }

    [HttpGet("{matchNo}")]
    public async Task<IActionResult> GetLiveMatch(int matchNo)
    {
        var data = await _liveService.GetLiveMatchAsync(matchNo);

        if (data == null)
            return NotFound();

        return Ok(data);
    }
}