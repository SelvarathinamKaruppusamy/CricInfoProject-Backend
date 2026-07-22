using AutoMapper;
using CricInfo.Application.DTOs.CompletedModule;
using CricInfo.Application.Interfaces.Repositories.CompletedModule;
using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Services.CompletedModule;
using CricInfo.Domain.Entities;

public class CompletedService : ICompletedService
{
    private readonly IMatchesRepository _matchRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IBattingRepository _battingRepository;
    private readonly IBowlingRepository _bowlingRepository;
    private readonly IMapper _mapper;

    public CompletedService(
        IMatchesRepository matchRepository,
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

        return await BuildCompletedMatch(match);
    }

    public async Task<List<CompletedMatchResponseDto>> GetCompletedMatchesAsync()
    {
        var matches = await _matchRepository.GetCompletedMatchesAsync();

        var result = new List<CompletedMatchResponseDto>();

        foreach (var match in matches)
        {
            result.Add(await BuildCompletedMatch(match));
        }

        return result;
    }

    private async Task<CompletedMatchResponseDto> BuildCompletedMatch(Match match)
    {
        var teams = await _teamRepository.GetTeamsByMatchNoAsync(match.matchNo);

        var response = _mapper.Map<CompletedMatchResponseDto>(match);

        response.teams = new List<CompletedTeamResponseDto>();

        foreach (var team in teams)
        {
            var teamDto = _mapper.Map<CompletedTeamResponseDto>(team);

            var batting = await _battingRepository
                .GetBattingByMatchNoAsync(match.matchNo, team.TeamId);

            var bowling = await _bowlingRepository
                .GetBowlingByMatchNoAsync(match.matchNo, team.TeamId);

            teamDto.batting = _mapper.Map<List<BattingResponseDto>>(batting);
            teamDto.bowling = _mapper.Map<List<BowlingResponseDto>>(bowling);

            response.teams.Add(teamDto);
        }

        return response;
    }
}