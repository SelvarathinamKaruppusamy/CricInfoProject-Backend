using AutoMapper;
using CricInfo.Application.DTOs.CompletedModule;
using CricInfo.Application.Interfaces.Repositories.CompletedModule;
using CricInfo.Application.Interfaces.Services.CompletedModule;
using CricInfo.Domain.Entities;

namespace CricInfo.Application.Services.CompletedModule
{
    public class CompletedService : ICompletedService
    {
        private readonly ICompletedRepository _completedRepository;
        private readonly IMapper _mapper;

        public CompletedService(
            ICompletedRepository completedRepository,
            IMapper mapper)
        {
            _completedRepository = completedRepository;
            _mapper = mapper;
        }

        public async Task<CompletedMatchResponseDto?> GetCompletedMatchAsync(int matchNo)
        {
            var match = await _completedRepository.GetCompletedMatchAsync(matchNo);

            if (match == null)
                return null;

            var teams = await _completedRepository.GetTeamsByMatchNoAsync(matchNo);

            var response = _mapper.Map<CompletedMatchResponseDto>(match);

            foreach (var team in teams)
            {
                var teamDto = _mapper.Map<CompletedTeamResponseDto>(team);

                var batting = await _completedRepository.GetBattingByMatchNoAsync(matchNo);

                var bowling = await _completedRepository.GetBowlingByMatchNoAsync(matchNo);

                teamDto.batting = _mapper.Map<List<BattingResponseDto>>(
                    batting.Where(x => x.TeamId == team.TeamId));

                teamDto.bowling = _mapper.Map<List<BowlingResponseDto>>(
                    bowling.Where(x => x.TeamId == team.TeamId));

                response.teams.Add(teamDto);
            }

            return response;
        }

        public async Task<List<CompletedMatchResponseDto>> GetCompletedMatchesAsync()
        {
            var matches = await _completedRepository.GetCompletedMatchesAsync();

            var matchNos = matches
                .Select(x => x.matchNo)
                .ToList();

            var teams = await _completedRepository.GetTeamsByMatchNosAsync(matchNos);

            var batting = await _completedRepository.GetBattingByMatchNosAsync(matchNos);

            var bowling = await _completedRepository.GetBowlingByMatchNosAsync(matchNos);

            var result = new List<CompletedMatchResponseDto>();

            foreach (var match in matches)
            {
                var matchDto = _mapper.Map<CompletedMatchResponseDto>(match);

                var matchTeams = teams
                    .Where(t => t.matchNo == match.matchNo)
                    .ToList();

                foreach (var team in matchTeams)
                {
                    var teamDto = _mapper.Map<CompletedTeamResponseDto>(team);

                    teamDto.batting = _mapper.Map<List<BattingResponseDto>>(
                        batting.Where(b =>
                            b.MatchNo == match.matchNo &&
                            b.TeamId == team.TeamId));

                    teamDto.bowling = _mapper.Map<List<BowlingResponseDto>>(
                        bowling.Where(b =>
                            b.MatchNo == match.matchNo &&
                            b.TeamId == team.TeamId));

                    matchDto.teams.Add(teamDto);
                }

                result.Add(matchDto);
            }

            return result;
        }
    }
}