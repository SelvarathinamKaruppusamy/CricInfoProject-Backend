using CricInfo.Application.DTOs.Live.RequestDto;
using CricInfo.Application.DTOs.Live.ResponseDto;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Domain.Entities;
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

    [HttpGet]
    public async Task<IActionResult> GetLiveMatch()
    {
        var data = await _liveService.GetLiveMatchAsync();

        if (data == null)
            return NotFound();

        return Ok(data);
    }

    [HttpGet("team/{teamId}/{matchNo}")]
    public async Task<ActionResult<Team>> GetTeam(int teamId, int matchNo)
    {
        var team = await _liveService.GetLiveTeamAsync(teamId, matchNo);
        if (team == null) return NotFound();
        return Ok(team);
    }
    [HttpGet("{playerId}/{teamId}/{matchNo}")]
    public async Task<ActionResult<Player>> GetPlayer(int playerId, int teamId, int matchNo)
    {
        var Player = await _liveService.GetLivePlayerAsync(playerId, teamId, matchNo);
        if (Player == null) return NotFound();
        return Ok(Player);
    }

    [HttpPut("Match/{matchNo}")]
    public async Task<ActionResult<string>> UpdateLiveMatch(int matchNo, [FromBody] MatchUpdateDto matchUpdateDto) {
        if (matchUpdateDto == null) return "The data is empty from the body";
        await _liveService.UpdateMatchAsync(matchNo, matchUpdateDto);
        return Ok(new
        {
            message = "Successful match updated"
        });
    }
    [HttpPut("Team/{teamId}/{matchNo}")]
    public async Task<string> UpdateLiveTeam(int teamId,int matchNo, [FromBody] TeamUpdateDto teamUpdateDto)
    {
        if(teamUpdateDto == null) return "The data is empty from the body";
        await _liveService.UpdateTeamAsync(teamId, matchNo, teamUpdateDto);
        return "Successfull Team updated";
    }
    [HttpPut("Player/{playerId}/{teamId}/{matchNo}")]
    public async Task<string> UpdateLivePlayer(int playerId,int teamId,int matchNo, [FromBody] PlayerUpdateDto playerUpdateDto)
    {
        if(playerUpdateDto == null) return "The data is empty from the body";
        await _liveService.UpdatePlayerAsync(playerId,teamId, matchNo, playerUpdateDto);
        return "Successfull Player updated";
    }
    [HttpPost("ball")]
    public async Task<IActionResult> ProcessBall(
     [FromBody] BallUpdateDto dto)
    {
        var result = await _liveService.ProcessBallAsync(dto);

        if (!result)
            return BadRequest(new
            {
                success = false,
                message = "Unable to update ball."
            });

        return Ok(new
        {
            success = true,
            message = "Ball Updated Successfully"
        });
    }
    [HttpPut("toss")]
    public async Task<IActionResult> UpdateToss([FromBody] TossDto dto)
    {
        var result = await _liveService.UpdateTossAsync(dto);

        if (!result)
            return BadRequest();

        return Ok("Toss Updated Successfully");
    }
    [HttpPost("start-match/{matchNo}")]
    public async Task<IActionResult> StartMatch(int matchNo)
    {
        var result = await _liveService.StartMatchAsync(matchNo);

        if (!result)
            return BadRequest("Unable to start match.");

        return Ok(new
        {
            success = true,
            message = "Match Started Successfully."
        });
    }

    [HttpPut("change-bowler")]
    public async Task<ActionResult<string>> ChangeBowler(ChangeBowlerDto dto)
    {
        var result = await _liveService.ChangeBowlerAsync(dto);

        if (!result)
            return BadRequest();

        return Ok(new
        {
            message = "Successful bowler changed"
        });
    }
    [HttpPut("{matchNo}/player-of-the-match")]
    public async Task<ActionResult<string>> UpdatePlayerOfTheMatch(
    int matchNo,
    PlayerOfTheMatchDto dto)
    {
        var result = await _liveService.UpdatePlayerOfTheMatchAsync(matchNo, dto);

        if (!result)
            return BadRequest();

        return Ok(new
        {
            message = "Successful Player Of the Match Updated"
        });
    }
    [HttpPost("start-second-innings/{matchNo}")]
    public async Task<IActionResult> StartSecondInnings(int matchNo)
    {
        var result = await _liveService.StartSecondInningsAsync(matchNo);

        if (!result)
            return BadRequest();

        return Ok("Second Innings Started Successfully");
    }
    [HttpPut("complete-match")]
    public async Task<IActionResult> CompleteMatch([FromBody] CompletedMatchDto dto)
    {
        var result = await _liveService.CompleteMatchAsync(dto);

        if (!result)
            return BadRequest();

        return Ok(new
        {
            message = "Match Completed Successfully"
        });
    }
    [HttpPost("promote/{matchNo}")]
    public async Task<IActionResult> PromoteUpcomingMatch(int matchNo)
    {
        var result = await _liveService.PromoteUpcomingMatchAsync(matchNo);

        if (!result)
            return BadRequest("Unable to promote match.");

        return Ok(new
        {
            success = true,
            message = "Match promoted successfully."
        });
    }
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingMatches()
    {
        var data = await _liveService.GetUpcomingMatchesAsync();

        if (data == null)
            return NotFound();

        return Ok(data);
    }
}