using CricInfo.Application.Interfaces.Services.PointsTableModule;
using Microsoft.AspNetCore.Mvc;

namespace CricInfo.API.Controllers.PointsTableModule;

[ApiController]
[Route("api/[controller]")]
public class PointsTableController : ControllerBase
{
    private readonly IPointsTableService _service;

    public PointsTableController(IPointsTableService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetLatest()
    {
        var result = await _service.GetLatestPointsTableAsync();
        return Ok(result);
    }

    [HttpGet("{matchNo}")]
    public async Task<IActionResult> GetByMatchNo(int matchNo)
    {
        var result = await _service.GetPointsTableAsync(matchNo);
        return Ok(result);
    }
}