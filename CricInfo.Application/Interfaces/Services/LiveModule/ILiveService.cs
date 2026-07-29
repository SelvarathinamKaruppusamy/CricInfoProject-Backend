using CricInfo.Application.DTOs.Live;
using CricInfo.Application.DTOs.Live.RequestDto;
using CricInfo.Application.DTOs.Live.ResponseDto;

namespace CricInfo.Application.Interfaces.Services.LiveModule;

public interface ILiveService
{
    Task<TeamDto?> GetLiveTeamAsync(int teamid, int MatchNo);
    Task<PlayerDto?> GetLivePlayerAsync(int playerid, int teamId, int matchNo);
    Task<MatchDto?> GetLiveMatchAsync();
    Task<bool> UpdateMatchAsync(int matchNo, MatchUpdateDto dto);

    Task<bool> UpdateTeamAsync(int teamId, int matchNo, TeamUpdateDto dto);

    Task<bool> UpdatePlayerAsync(int playerId, int teamId, int matchNo, PlayerUpdateDto dto);
    Task<bool> ProcessBallAsync(BallUpdateDto dto);
    Task<bool> StartMatchAsync(int matchNo);
    Task<bool> UpdateTossAsync(TossDto dto);
    Task<bool> ChangeBowlerAsync(ChangeBowlerDto dto);
    //Task<(bool Success, string Message)> ChangeBowlerAsync(ChangeBowlerDto dto);
    Task<bool> UpdatePlayerOfTheMatchAsync(
    int matchNo,
    PlayerOfTheMatchDto dto);
    Task<bool> StartSecondInningsAsync(int matchNo);
    Task<bool> CompleteMatchAsync(CompletedMatchDto dto);
    Task<bool> PromoteUpcomingMatchAsync(int matchNo);
    Task<List<MatchDto>> GetUpcomingMatchesAsync();

}