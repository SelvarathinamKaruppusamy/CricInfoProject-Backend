using CricInfo.Application.DTOs.PointsTable;
using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Services.PointsTableModule;
using CricInfo.Domain.Entities;

namespace CricInfo.Application.Services.PointsTableModule;

public class PointsTableService : IPointsTableService
{
    private readonly ITeamRepository _teamRepository;

 
    private const decimal FullOversQuota = 20m;

    private const int AllOutWickets = 10;

    public PointsTableService(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }

    public async Task<List<PointsTableDto>> GetLatestPointsTableAsync()
    {
        var teams = await _teamRepository.GetAllTeamsAsync();

        return CalculatePointsTable(teams);
    }

    public async Task<List<PointsTableDto>> GetPointsTableAsync(int matchNo)
    {
        var teams = await _teamRepository.GetTeamsUntilMatchAsync(matchNo);

        return CalculatePointsTable(teams);
    }

    private List<PointsTableDto> CalculatePointsTable(List<Team> teams)
    {
       
        var matchGroups = teams
            .GroupBy(x => x.matchNo)
            .ToDictionary(g => g.Key, g => g.ToList());

        var groupedTeams = teams.GroupBy(x => x.TeamId);

        var result = new List<PointsTableDto>();

        foreach (var group in groupedTeams)
        {
            var teamMatches = group.ToList();

            var latest = teamMatches
                .OrderByDescending(x => x.matchNo)
                .First();

            decimal runsScored = 0m;
            decimal oversFaced = 0m;
            decimal runsConceded = 0m;
            decimal oversBowled = 0m;

            foreach (var innings in teamMatches)
            {
                runsScored += innings.runs ?? 0;
                oversFaced += CalculateEffectiveOvers(
                    ballsFaced: innings.balls ?? 0,
                    wicketsLost: innings.wickets ?? 0);

                if (!matchGroups.TryGetValue(innings.matchNo, out var matchRows))
                {
                    continue;
                }

                var opponent = matchRows
                    .FirstOrDefault(t => t.TeamId != innings.TeamId);

                if (opponent == null)
                {
                    continue;
                }

                runsConceded += opponent.runs ?? 0;
                oversBowled += CalculateEffectiveOvers(
                    ballsFaced: opponent.balls ?? 0,
                    wicketsLost: opponent.wickets ?? 0);
            }

            decimal runRateFor = oversFaced > 0
                ? runsScored / oversFaced
                : 0m;

            decimal runRateAgainst = oversBowled > 0
                ? runsConceded / oversBowled
                : 0m;

            decimal nrr = runRateFor - runRateAgainst;

            result.Add(new PointsTableDto
            {
                TeamId = latest.TeamId,
                TeamName = latest.fullName,
                ShortName = latest.shortName,
                Logo = latest.logo,
                Played = latest.totalMatch ?? 0,
                Wins = latest.winCount ?? 0,
                Losses = latest.lossCount ?? 0,
                Points = (latest.winCount ?? 0) * 2,
                NRR = Math.Round(nrr, 3),
                Form = ParseForm(latest.matchStatus)
            });
        }

        return result
            .OrderByDescending(x => x.Points)
            .ThenByDescending(x => x.NRR)
            .ToList();
    }

    private decimal CalculateEffectiveOvers(int ballsFaced, int wicketsLost)
    {
        if (wicketsLost >= AllOutWickets)
        {
            return FullOversQuota;
        }

        return ballsFaced > 0
            ? (decimal)ballsFaced / 6m
            : 0m;
    }

    private List<bool> ParseForm(string? matchStatus)
    {
        if (string.IsNullOrEmpty(matchStatus))
        {
            return new List<bool>();
        }

        return matchStatus
            .Split(',')
            .Select(x => bool.Parse(x.Trim()))
            .ToList();
    }
}