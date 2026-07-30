using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class MatchRepository(
    CricDbContext _context,
    ILogger<MatchRepository> _logger)
    : IMatchRepository
{
    public async Task<Match?> GetMatchByMatchNoAsync(int matchNo)
    {
        _logger.LogInformation(
            "Fetching match from database. MatchNo: {MatchNo}",
            matchNo);

        var match = await _context.Matches
            .FirstOrDefaultAsync(x => x.matchNo == matchNo);

        if (match == null)
        {
            _logger.LogWarning(
                "Match not found in database. MatchNo: {MatchNo}",
                matchNo);

            throw new KeyNotFoundException($"Match with Id {matchNo} was not found.");
        }

        _logger.LogInformation(
            "Match retrieved successfully. MatchNo: {MatchNo}",
            matchNo);

        return match;
    }

    public async Task<List<Match>> GetLiveMatchesAsync()
    {
        _logger.LogInformation(
            "Fetching all live matches from database.");

        var matches = await _context.Matches
            .Where(x => x.status == "LIVE")
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {Count} live match(es).",
            matches.Count);

        return matches;
    }

    public async Task<Match?> GetCurrentLiveMatchAsync()
    {
        _logger.LogInformation(
            "Fetching current live match from database.");

        var match = await _context.Matches
            .FirstOrDefaultAsync(x => x.status == "LIVE");

        if (match == null)
        {
            _logger.LogWarning(
                "No current live match found.");

            return null;
        }

        _logger.LogInformation(
            "Current live match retrieved. MatchNo: {MatchNo}",
            match.matchNo);

        return match;
    }

    public async Task<List<Match>> GetUpcomingMatchesAsync()
    {
        _logger.LogInformation(
            "Fetching upcoming matches from database.");

        var matches = await _context.Matches
            .Where(x => x.status == "UPCOMING")
            .OrderBy(x => x.matchNo)
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {Count} upcoming match(es).",
            matches.Count);

        return matches;
    }

    public async Task UpdateMatchAsync(Match match)
    {
        _logger.LogInformation(
            "Saving changes for MatchNo: {MatchNo}",
            match.matchNo);

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Database updated successfully for MatchNo: {MatchNo}",
            match.matchNo);
    }
}