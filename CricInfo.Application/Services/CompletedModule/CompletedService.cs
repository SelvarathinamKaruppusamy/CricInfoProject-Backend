using AutoMapper;
using CricInfo.Application.DTOs.CompletedModule;
using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Services.CompletedModule;

namespace CricInfo.Application.Services.CompletedModule;

public class CompletedService : ICompletedService
{
    private readonly IMatchRepository _matchRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IBattingRepository _battingRepository;
    private readonly IBowlingRepository _bowlingRepository;
    private readonly IMapper _mapper;

    public CompletedService(
        IMatchRepository matchRepository,
        ITeamRepository teamRepository,
        IBattingRepository battingRepository,
        IBowlingRepository bowlingRepository,
        IMapper mapper)
    {
        _matchRepository = matchRepository;
        _teamRepository = teamRepository;
        _battingRepository = battingRepository;
        _bowlingRepository = bowlingRepository;
        _mapper = mapper;
    }

    public async Task<CompletedMatchResponseDto?> GetCompletedMatchAsync(int matchNo)
    {
        var match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);

        if (match == null)
            return null;

        if (!string.Equals(match.status, "COMPLETED", StringComparison.OrdinalIgnoreCase))
            return null;
        var teams = await _teamRepository.GetTeamsByMatchNoAsync(matchNo);

        var response = _mapper.Map<CompletedMatchResponseDto>(match);

        response.teams = new List<CompletedTeamResponseDto>();

        foreach (var team in teams)
        {
            var teamDto = _mapper.Map<CompletedTeamResponseDto>(team);

            var batting = await _battingRepository
                .GetBattingByMatchNoAsync(matchNo, team.TeamId);

            var bowling = await _bowlingRepository
                .GetBowlingByMatchNoAsync(matchNo, team.TeamId);

            teamDto.batting = _mapper.Map<List<BattingResponseDto>>(batting);

            teamDto.bowling = _mapper.Map<List<BowlingResponseDto>>(bowling);

            response.teams.Add(teamDto);
        }
        return response;
    }
}