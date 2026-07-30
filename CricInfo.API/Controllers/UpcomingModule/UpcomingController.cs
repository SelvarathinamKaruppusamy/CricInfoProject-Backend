using CricInfo.Application.DTOs.Upcoming;
using CricInfo.Application.Interfaces.Services.Upcoming;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CricInfo.API.Controllers.UpcomingModule
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpcomingController : ControllerBase
    {
        private readonly IUpcomingService _upcomingService;

        public UpcomingController(IUpcomingService upcomingService)
        {
        _upcomingService = upcomingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUpcomingMatches()
        {
            var result = await _upcomingService.GetUpcomingMatchesAsync();
            return Ok(result);
        }

        [HttpGet("{matchNo}")]
        public async Task<IActionResult> GetMatchByMatchNo(int matchNo)
        {
            var result = await _upcomingService.GetUpcomingMatchByIdAsync(matchNo);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut("{matchNo}")]
        public async Task<IActionResult> UpdateMatch(int matchNo,[FromBody] UpdateMatchDTO dto)
        {
            var update = await _upcomingService.UpdateUpcomingMatchAsync(matchNo, dto);

            if (!update)
            {
                return NotFound($"Match {matchNo} is not found.");
            }
            return Ok( new {Message="Match Updated Successfully" });
        }
    }
}
