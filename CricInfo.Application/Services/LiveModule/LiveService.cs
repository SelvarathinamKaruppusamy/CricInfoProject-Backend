using AutoMapper;
using CricInfo.Application.DTOs.Live;
using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Services.LiveModule;

namespace CricInfo.Application.Services.LiveModule;

public class LiveService : ILiveService
{
    private readonly IMatchRepository _matchRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IMapper _mapper;

    public LiveService(
        IMatchRepository matchRepository,
        ITeamRepository teamRepository,
        IPlayerRepository playerRepository,
        IMapper mapper)
    {
        _matchRepository = matchRepository;
        _teamRepository = teamRepository;
        _playerRepository = playerRepository;
        _mapper = mapper;
    }

    public async Task<LiveMatchResponseDto?> GetLiveMatchAsync(int matchNo)
    {
        var match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);

        if (match == null)
            return null;

        var teams = await _teamRepository.GetTeamsByMatchNoAsync(matchNo);

        var players = await _playerRepository.GetPlayersByMatchNoAsync(matchNo);

        var response = new LiveMatchResponseDto
        {
            Match = _mapper.Map<MatchDto>(match)
        };

        foreach (var team in teams)
        {
            var teamDto = _mapper.Map<TeamDto>(team);

            teamDto.Players = _mapper.Map<List<PlayerDto>>
            (
                players.Where(x => x.TeamId == team.TeamId).ToList()
            );

            response.Teams.Add(teamDto);
        }

        return response;
    }
}