using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Domain.Entities;
using CricInfo.Infrastructure.presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CricInfo.Infrastructure.Repositories.LiveModule;

public class PlayerRepository(
    CricDbContext _context,
    ILogger<PlayerRepository> _logger)
    : IPlayerRepository
{
    public async Task<Player?> GetPlayerByIdAsync(
        int playerId,
        int teamId,
        int matchNo)
    {
        _logger.LogInformation(
            "Fetching player from database. MatchNo: {MatchNo}, TeamId: {TeamId}, PlayerId: {PlayerId}",
            matchNo,
            teamId,
            playerId);

        var player = await _context.Players.FirstOrDefaultAsync(
            x => x.playerId == playerId &&
                 x.TeamId == teamId &&
                 x.matchNo == matchNo);

        if (player == null)
        {
            _logger.LogWarning(
                "Player not found in database. MatchNo: {MatchNo}, TeamId: {TeamId}, PlayerId: {PlayerId}",
                matchNo,
                teamId,
                playerId);

            throw new KeyNotFoundException($"The Player from the PlayerId {playerId} was not found.");
        }

        _logger.LogInformation(
            "Player retrieved successfully. Player: {PlayerName}",
            player.name);

        return player;
    }

    public async Task<List<Player>> GetPlayersByMatchNoAsync(int matchNo)
    {
        _logger.LogInformation(
            "Fetching players for MatchNo: {MatchNo}",
            matchNo);

        var players = await _context.Players
            .Where(x => x.matchNo == matchNo)
            .OrderBy(x => x.TeamId)
            .ThenBy(x => x.playerId)
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {Count} players for MatchNo: {MatchNo}",
            players.Count,
            matchNo);

        return players;
    }

    public async Task<List<Player>> GetPlayersByTeamAsync(
        int teamId,
        int matchNo)
    {
        _logger.LogInformation(
            "Fetching players for TeamId: {TeamId}, MatchNo: {MatchNo}",
            teamId,
            matchNo);

        var players = await _context.Players
            .Where(x => x.TeamId == teamId &&
                        x.matchNo == matchNo)
            .OrderBy(x => x.playerId)
            .ToListAsync();

        _logger.LogInformation(
            "Retrieved {Count} players for TeamId: {TeamId}",
            players.Count,
            teamId);

        return players;
    }

    public async Task<Player?> GetNextBatterAsync(
        int teamId,
        int matchNo)
    {
        _logger.LogInformation(
            "Fetching next batter. TeamId: {TeamId}, MatchNo: {MatchNo}",
            teamId,
            matchNo);

        var batter = await _context.Players
            .Where(x =>
                x.TeamId == teamId &&
                x.matchNo == matchNo &&
                x.status == "Yet To Play")
            .OrderBy(x => x.playerId)
            .FirstOrDefaultAsync();

        if (batter == null)
        {
            _logger.LogWarning(
                "No next batter available. TeamId: {TeamId}, MatchNo: {MatchNo}",
                teamId,
                matchNo);

            return null;
        }

        _logger.LogInformation(
            "Next batter retrieved: {PlayerName}",
            batter.name);

        return batter;
    }

    public async Task UpdatePlayerAsync(Player player)
    {
        _logger.LogInformation(
            "Saving player changes. PlayerId: {PlayerId}",
            player.playerId);

        await _context.SaveChangesAsync();

        _logger.LogInformation(
            "Player changes saved successfully. PlayerId: {PlayerId}",
            player.playerId);
    }
    public async Task DeletePlayersByMatchNoAsync(int matchNo)
    {
        var players = await _context.Players
            .Where(x => x.matchNo == matchNo)
            .ToListAsync();

        _context.Players.RemoveRange(players);
    }
}