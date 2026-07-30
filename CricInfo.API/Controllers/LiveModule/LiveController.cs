using CricInfo.API.Filters;
using CricInfo.Application.DTOs.Live.RequestDto;
using CricInfo.Application.DTOs.Live.ResponseDto;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CricInfo.API.Controllers.LiveModule;

[ApiController]
[Route("api/[controller]")]
public class LiveController(ILiveService _liveService) : ControllerBase
{
    // This Action Method is used to access the Live Match in the Database
    [HttpGet]
    public async Task<IActionResult> GetLiveMatch()
    {
        var data = await _liveService.GetLiveMatchAsync();

        if (data == null)
            return NotFound();

        return Ok(data);
    }



    // This Action Method is used to Update the specific Match data in the Database using (primary keys - matchNo)

    [HttpPut("Match/{matchNo}")]
    public async Task<ActionResult<string>> UpdateLiveMatch(int matchNo, [FromBody] MatchUpdateDto matchUpdateDto)
    {
        if (matchUpdateDto == null) return "The data is empty from the body";
        await _liveService.UpdateMatchAsync(matchNo, matchUpdateDto);
        return Ok(new
        {
            message = "Successful match updated"
        });
    }


    // This Action Method is used to Update the every ball from the frontend admin panel to match ,teams,players data 

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


    // This Action Method is used to Update the toss data for specific Match data in the Database 

    [HttpPut("toss")]
    public async Task<IActionResult> UpdateToss([FromBody] TossDto dto)
    {
        var result = await _liveService.UpdateTossAsync(dto);

        if (!result)
            return BadRequest();

        return Ok("Toss Updated Successfully");
    }


    // This Action Method is used to Update the specific Match data like current Innings and toss winner and batting teams striker and non striker and current bowler in the Database

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


    // This Action Method is used to Update the every over new bowler in the Database

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


    // This Action Method is used to Update the specific Match data after completed the live for player of the match data for completed match in the Database 

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


    // This Action Method is used to Update the specific Match data for secondInnings in the Database 

    [HttpPost("start-second-innings/{matchNo}")]
    public async Task<IActionResult> StartSecondInnings(int matchNo)
    {
        var result = await _liveService.StartSecondInningsAsync(matchNo);

        if (!result)
            return BadRequest();

        return Ok("Second Innings Started Successfully");
    }


    // This Action Method is used to Update the Live Match data to Completed Match status in the Database

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


    // This Action Method is used to Update the next upcoming Match data  to Live match in the Database using (primary keys - matchNo)

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


    // This Action Method is used to get the next upcoming Match data

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingMatches()
    {
        var data = await _liveService.GetUpcomingMatchesAsync();

        if (data == null)
            return NotFound();

        return Ok(data);
    }
}