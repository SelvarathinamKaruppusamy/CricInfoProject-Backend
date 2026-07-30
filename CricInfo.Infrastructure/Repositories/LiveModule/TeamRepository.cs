using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CricInfo.Infrastructure.Repositories.LiveModule;

public class TeamRepository(
    CricDbContext _context,
    ILogger<TeamRepository> _logger)
    : ITeamRepository
{
    public async Task<Team?> GetTeamByIdAsync(
        int teamId,
        int matchNo)
    {
        _logger.LogInformation(
            "Fetching team from database. MatchNo: {MatchNo}, TeamId: {TeamId}",
            matchNo,
            teamId);

        var team = await _context.Teams.FirstOrDefaultAsync(
            x => x.TeamId == teamId &&
                 x.matchNo == matchNo);

        if (team == null)
        {
            _logger.LogWarning(
                "Team not found in database. MatchNo: {MatchNo}, TeamId: {TeamId}",
                matchNo,
                teamId);

            throw new KeyNotFoundException($"The Team with teamId {teamId} Not found");
        }

        _logger.LogInformation(
            "Team retrieved successfully. Team: {TeamName}",
            team.shortName);

        return team;
    }

    public async Task<List<Team>> GetTeamsByMatchNoAsync(int matchNo)
    {
        _logger.LogInformation(
            "Fetching teams for MatchNo: {MatchNo}",
            matchNo);

        var teams = await _context.Teams
            .Where(x => x.matchNo == matchNo)
            .OrderBy(x => x.TeamId)
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {Count} team(s) for MatchNo: {MatchNo}",
            teams.Count,
            matchNo);

        return teams;
    }

    public async Task UpdateTeamAsync(Team team)
    {
        _logger.LogInformation(
            "Saving team changes. TeamId: {TeamId}, MatchNo: {MatchNo}",
            team.TeamId,
            team.matchNo);

        await _context.SaveChangesAsync();
    public async Task<List<Team>> GetAllTeamsAsync()
    {
        return await _context.Teams.ToListAsync();
    }

    public async Task<List<Team>> GetTeamsUntilMatchAsync(int matchNo)
    {
        return await _context.Teams
            .Where(x => x.matchNo <= matchNo)
            .ToListAsync();
    }
}