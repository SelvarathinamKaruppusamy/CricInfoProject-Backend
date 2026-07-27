using CricInfo.Application.Interfaces.Services.CompletedModule;
using Microsoft.AspNetCore.Mvc;

namespace CricInfo.API.Controllers.CompletedModule;

using CricInfo.Application.Services.CompletedModule;
using System.Diagnostics;


[ApiController]
[Route("api/[controller]")]
public class CompletedController : ControllerBase
{
    private readonly ICompletedService _completedService;

    public CompletedController(ICompletedService completedService)
    {
        _completedService = completedService;
    }

    // GET: api/completed
   [HttpGet]
    public async Task<IActionResult> GetCompletedMatches()
    {
        var result = await _completedService.GetCompletedMatchesAsync();
        return Ok(result);
    }


    // GET: api/completed/1
    [HttpGet("{matchNo}")]
    public async Task<IActionResult> GetCompletedMatch(int matchNo)
    {
        var result = await _completedService.GetCompletedMatchAsync(matchNo);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

}