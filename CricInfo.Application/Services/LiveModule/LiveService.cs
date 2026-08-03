using AutoMapper;

using CricInfo.API.Memory;

using CricInfo.Application.DTOs.Live.RequestDto;

using CricInfo.Application.DTOs.Live.ResponseDto;
using CricInfo.Application.Interfaces.Repositories.CompletedModule;
using CricInfo.Application.Interfaces.Repositories.LiveModule;

using CricInfo.Application.Interfaces.Services.LiveModule;

using CricInfo.Domain.Entities;

using Microsoft.Extensions.Logging;

namespace CricInfo.Application.Services.LiveModule;

public class LiveService(IMatchRepository _matchRepository, ITeamRepository _teamRepository, IPlayerRepository _playerRepository, IMapper _mapper, IUnitOfWork _unitOfWork, ILogger<LiveService> _logger, IBattingRepository _battingRepository,
    IBowlingRepository _bowlingRepository) : ILiveService

{

    public async Task<MatchDto?> GetLiveMatchAsync()

    {

        _logger.LogInformation("Fetching current live match.");

        var match = await _matchRepository.GetCurrentLiveMatchAsync();

        if (match == null)

        {

            _logger.LogWarning("No live match found in the database.");

            return null;

        }

        _logger.LogInformation(

            "Live match found. Match No: {MatchNo}",

            match.matchNo);

        var matchNo = match.matchNo;

        var teams = await _teamRepository.GetTeamsByMatchNoAsync(matchNo);

        _logger.LogInformation(

            "Retrieved {TeamCount} teams for Match {MatchNo}.",

            teams.Count,

            matchNo);

        var players = await _playerRepository.GetPlayersByMatchNoAsync(matchNo);

        _logger.LogInformation(

            "Retrieved {PlayerCount} players for Match {MatchNo}.",

            players.Count,

            matchNo);

        var matchDto = _mapper.Map<MatchDto>(match);

        foreach (var team in teams)

        {

            var teamDto = _mapper.Map<TeamDto>(team);

            teamDto.Players = _mapper.Map<List<PlayerDto>>(

                players.Where(p => p.TeamId == team.TeamId).ToList()

            );

            matchDto.Teams.Add(teamDto);

            _logger.LogInformation(

                "Mapped Team {TeamName} with {PlayerCount} players.",

                team.shortName,

                teamDto.Players.Count);

        }

        matchDto.FirstInningsBalls =

            MatchBallStore.FirstInningsBalls.GetValueOrDefault(

                matchNo,

                new List<string>());

        matchDto.SecondInningsBalls =

            MatchBallStore.SecondInningsBalls.GetValueOrDefault(

                matchNo,

                new List<string>());

        _logger.LogInformation(

            "Loaded ball history. First Innings: {FirstBalls}, Second Innings: {SecondBalls}.",

            matchDto.FirstInningsBalls.Count,

            matchDto.SecondInningsBalls.Count);

        _logger.LogInformation(

            "Successfully prepared Live Match response for Match {MatchNo}.",

            matchNo);

        return matchDto;

    }

    public async Task<PlayerDto?> GetLivePlayerAsync(

    int playerId,

    int teamId,

    int matchNo)

    {

        _logger.LogInformation(

            "Fetching player. MatchNo: {MatchNo}, TeamId: {TeamId}, PlayerId: {PlayerId}",

            matchNo,

            teamId,

            playerId);

        var player = await _playerRepository.GetPlayerByIdAsync(

            playerId,

            teamId,

            matchNo);

        if (player == null)

        {

            _logger.LogWarning(

                "Player not found. MatchNo: {MatchNo}, TeamId: {TeamId}, PlayerId: {PlayerId}",

                matchNo,

                teamId,

                playerId);

            return null;

        }

        _logger.LogInformation(

            "Player '{PlayerName}' retrieved successfully.",

            player.name);

        return _mapper.Map<PlayerDto>(player);

    }

    public async Task<TeamDto?> GetLiveTeamAsync(

     int teamId,

     int matchNo)

    {

        _logger.LogInformation(

            "Fetching team. MatchNo: {MatchNo}, TeamId: {TeamId}",

            matchNo,

            teamId);

        var team = await _teamRepository.GetTeamByIdAsync(

            teamId,

            matchNo);

        if (team == null)

        {

            _logger.LogWarning(

                "Team not found. MatchNo: {MatchNo}, TeamId: {TeamId}",

                matchNo,

                teamId);

            return null;

        }

        _logger.LogInformation(

            "Team '{TeamName}' retrieved successfully.",

            team.shortName);

        return _mapper.Map<TeamDto>(team);

    }

    public async Task<bool> UpdateMatchAsync(

    int matchNo,

    MatchUpdateDto dto)

    {

        _logger.LogInformation(

            "Updating match. MatchNo: {MatchNo}",

            matchNo);

        var match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);

        if (match == null)

        {

            _logger.LogWarning(

                "Match not found. MatchNo: {MatchNo}",

                matchNo);

            return false;

        }

        _mapper.Map(dto, match);

        _logger.LogInformation(

            "Mapped updated values to Match {MatchNo}.",

            matchNo);

        await _matchRepository.UpdateMatchAsync(match);

        _logger.LogInformation(

            "Match updated successfully. MatchNo: {MatchNo}",

            matchNo);

        return true;

    }

    public async Task<bool> UpdatePlayerAsync(

     int playerId,

     int teamId,

     int matchNo,

     PlayerUpdateDto dto)

    {

        _logger.LogInformation(

            "Updating player. MatchNo: {MatchNo}, TeamId: {TeamId}, PlayerId: {PlayerId}",

            matchNo,

            teamId,

            playerId);

        var player = await _playerRepository.GetPlayerByIdAsync(

            playerId,

            teamId,

            matchNo);

        if (player == null)

        {

            _logger.LogWarning(

                "Player not found. MatchNo: {MatchNo}, TeamId: {TeamId}, PlayerId: {PlayerId}",

                matchNo,

                teamId,

                playerId);

            return false;

        }

        _mapper.Map(dto, player);

        _logger.LogInformation(

            "Mapped updated values to Player '{PlayerName}'.",

            player.name);

        await _playerRepository.UpdatePlayerAsync(player);

        _logger.LogInformation(

            "Player updated successfully. MatchNo: {MatchNo}, TeamId: {TeamId}, PlayerId: {PlayerId}",

            matchNo,

            teamId,

            playerId);

        return true;

    }

    public async Task<bool> UpdateTeamAsync(

    int teamId,

    int matchNo,

    TeamUpdateDto dto)

    {

        _logger.LogInformation(

            "Updating team. MatchNo: {MatchNo}, TeamId: {TeamId}",

            matchNo,

            teamId);

        var team = await _teamRepository.GetTeamByIdAsync(

            teamId,

            matchNo);

        if (team == null)

        {

            _logger.LogWarning(

                "Team not found. MatchNo: {MatchNo}, TeamId: {TeamId}",

                matchNo,

                teamId);

            return false;

        }

        _mapper.Map(dto, team);

        _logger.LogInformation(

            "Mapped updated values to Team '{TeamName}'.",

            team.shortName);

        await _teamRepository.UpdateTeamAsync(team);

        _logger.LogInformation(

            "Team updated successfully. MatchNo: {MatchNo}, TeamId: {TeamId}",

            matchNo,

            teamId);

        return true;

    }

    public async Task<bool> ProcessBallAsync(BallUpdateDto dto)

    {

        _logger.LogInformation(

    "Processing ball. MatchNo: {MatchNo}, Ball: {BallResult}",

    dto.matchNo,

    dto.ballResult);

        var match = await _matchRepository.GetMatchByMatchNoAsync(dto.matchNo);

        if (match == null)

        {

            _logger.LogWarning(

                "Match not found. MatchNo: {MatchNo}",

                dto.matchNo);

            return false;

        }

        var battingTeam = await _teamRepository.GetTeamByIdAsync(

     match.currentBattingTeamIndex!.Value,

     match.matchNo);

        var bowlingTeam = await _teamRepository.GetTeamByIdAsync(

            match.currentBowlingTeamIndex!.Value,

            match.matchNo);

        var striker = await _playerRepository.GetPlayerByIdAsync(

            match.strikerPlayerId!.Value,

            battingTeam!.TeamId,

            match.matchNo);

        var nonStriker = await _playerRepository.GetPlayerByIdAsync(

            match.nonStrikerPlayerId!.Value,

            battingTeam.TeamId,

            match.matchNo);

        var bowler = await _playerRepository.GetPlayerByIdAsync(

            match.currentBowlerPlayerId!.Value,

            bowlingTeam!.TeamId,

            match.matchNo);

        _logger.LogInformation(

    "Current State | Innings: {Innings} | Batting: {BattingTeam} {Runs}/{Wickets} | Bowling: {BowlingTeam}",

    match.currentInnings,

    battingTeam.shortName,

    battingTeam.runs,

    battingTeam.wickets,

    bowlingTeam.shortName);

        if (battingTeam == null ||

            bowlingTeam == null ||

            striker == null ||

            nonStriker == null ||

            bowler == null)

        {

            return false;

        }

        switch (dto.ballResult)

        {

            case "0":

                _logger.LogInformation("Dot ball by {Bowler} to {Batter}.", bowler.name, striker.name);

                UpdateLegalBall(battingTeam, striker, bowler);

                UpdateStrikeRate(striker);

                UpdateEconomy(bowler);

                HandleOverCompletion(match, battingTeam);

                break;

            case "1":

                _logger.LogInformation("{Batter} scored 1 run.", striker.name);

                UpdateLegalBall(battingTeam, striker, bowler);

                AddRuns(battingTeam, striker, bowler, 1);

                UpdateScore(battingTeam);

                UpdateStrikeRate(striker);

                UpdateEconomy(bowler);

                SwapStrike(match);

                HandleOverCompletion(match, battingTeam);

                break;

            case "2":

                _logger.LogInformation("{Batter} scored 2 runs.", striker.name);

                UpdateLegalBall(battingTeam, striker, bowler);

                AddRuns(battingTeam, striker, bowler, 2);

                UpdateScore(battingTeam);

                UpdateStrikeRate(striker);

                UpdateEconomy(bowler);

                HandleOverCompletion(match, battingTeam);

                break;

            case "3":

                _logger.LogInformation("{Batter} scored 3 runs.", striker.name);

                UpdateLegalBall(battingTeam, striker, bowler);

                AddRuns(battingTeam, striker, bowler, 3);

                UpdateScore(battingTeam);

                UpdateStrikeRate(striker);

                UpdateEconomy(bowler);

                SwapStrike(match);

                HandleOverCompletion(match, battingTeam);

                break;

            case "4":

                _logger.LogInformation("FOUR! {Batter} hit a boundary.", striker.name);

                UpdateLegalBall(battingTeam, striker, bowler);

                AddRuns(battingTeam, striker, bowler, 4);

                striker.fours++;

                UpdateScore(battingTeam);

                UpdateStrikeRate(striker);

                UpdateEconomy(bowler);

                HandleOverCompletion(match, battingTeam);

                break;

            case "6":

                _logger.LogInformation("SIX! {Batter} cleared the boundary.", striker.name);

                UpdateLegalBall(battingTeam, striker, bowler);

                AddRuns(battingTeam, striker, bowler, 6);

                striker.sixes++;

                UpdateScore(battingTeam);

                UpdateStrikeRate(striker);

                UpdateEconomy(bowler);

                HandleOverCompletion(match, battingTeam);

                break;

            case "WD":

                _logger.LogInformation("Wide ball by {Bowler}.", bowler.name);

                HandleWide(battingTeam, bowler);

                break;

            case "NB":

                _logger.LogInformation("No Ball by {Bowler}.", bowler.name);

                HandleNoBall(battingTeam, bowler);

                break;

            case "W":

                _logger.LogInformation("WICKET! {Batter} dismissed by {Bowler}.", striker.name, bowler.name);

                UpdateLegalBall(battingTeam, striker, bowler);

                await HandleWicket(battingTeam, striker, bowler, match);

                UpdateStrikeRate(striker);

                UpdateEconomy(bowler);

                HandleOverCompletion(match, battingTeam);

                break;

            default:

                return false;

        }

        if (match.currentInnings == 1 &&

    IsFirstInningsCompleted(battingTeam))

        {

            _logger.LogInformation("First innings completed. Score: {Team} {Runs}/{Wickets}", battingTeam.shortName, battingTeam.runs, battingTeam.wickets);

            await StartSecondInningsAsync(match.matchNo);

            await _unitOfWork.SaveChangesAsync();

            return true;

        }

        StoreBall(match.matchNo, match.currentInnings!.Value, dto.ballResult);

        _logger.LogInformation("Ball stored in MatchBallStore. MatchNo: {MatchNo}", match.matchNo);

        if (match.currentInnings == 2 && IsMatchCompleted(match, battingTeam, bowlingTeam))

        {

            _logger.LogInformation("Match completed. Result: {Result}", match.result);

            await FinishMatch(match, battingTeam, bowlingTeam);

        }

        _logger.LogInformation("Saving match updates.");

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Ball processed successfully.");

        return true;

    }

    private void HandleWide(

    Team battingTeam,

    Player bowler)

    {

        battingTeam.runs++;

        battingTeam.extras++;

        bowler.runsConceded++;

        UpdateScore(battingTeam);

        UpdateEconomy(bowler);

        _logger.LogInformation("Wide ball processed. Team: {Team}, Score: {Runs}/{Wickets}, Extras: {Extras}, Bowler: {Bowler}", battingTeam.shortName, battingTeam.runs, battingTeam.wickets, battingTeam.extras, bowler.name);

    }

    private void HandleNoBall(

    Team battingTeam,

    Player bowler)

    {

        battingTeam.runs++;

        battingTeam.extras++;

        bowler.runsConceded++;

        UpdateScore(battingTeam);

        UpdateEconomy(bowler);

        _logger.LogInformation(

            "No Ball processed. Team: {Team}, Score: {Runs}/{Wickets}, Extras: {Extras}, Bowler: {Bowler}",

            battingTeam.shortName,

            battingTeam.runs,

            battingTeam.wickets,

            battingTeam.extras,

            bowler.name);

    }

    private async Task HandleWicket(

        Team battingTeam,

        Player striker,

        Player bowler,

        Match match)

    {

        _logger.LogInformation(

            "Wicket! Batter: {Batter}, Bowler: {Bowler}",

            striker.name,

            bowler.name);

        battingTeam.wickets++;

        striker.status = "Out";

        bowler.wickets++;

        var nextBatter = await _playerRepository.GetNextBatterAsync(

            battingTeam.TeamId,

            match.matchNo);

        if (nextBatter == null)

        {

            _logger.LogInformation(

                "No batsmen remaining. Team {Team} is all out at {Runs}/{Wickets}.",

                battingTeam.shortName,

                battingTeam.runs,

                battingTeam.wickets);

            UpdateScore(battingTeam);

            return;

        }

        nextBatter.status = "Batting";

        _logger.LogInformation(

            "New batter {NextBatter} has come to the crease.",

            nextBatter.name);

        var nonStriker = await _playerRepository.GetPlayerByIdAsync(

            match.nonStrikerPlayerId!.Value,

            battingTeam.TeamId,

            match.matchNo);

        if (nonStriker != null)

        {

            nonStriker.status = "Batting";

            _logger.LogInformation(

                "Non-striker remains {NonStriker}.",

                nonStriker.name);

        }

        match.strikerPlayerId = nextBatter.playerId;

        UpdateScore(battingTeam);

        _logger.LogInformation(

            "Score after wicket: {Team} {Runs}/{Wickets}.",

            battingTeam.shortName,

            battingTeam.runs,

            battingTeam.wickets);

    }

    private void SwapStrike(Match match)

    {

        var temp = match.strikerPlayerId;

        match.strikerPlayerId = match.nonStrikerPlayerId;

        match.nonStrikerPlayerId = temp;

    }

    private void UpdateLegalBall(

       Team battingTeam,

       Player striker,

       Player bowler)

    {

        // Batter

        striker.balls++;

        if (striker.balls == 1)

        {

            striker.status = "Batting";

        }

        bowler.bowlingBalls++;

        int completedOvers = bowler.bowlingBalls / 6;

        int currentBalls = bowler.bowlingBalls % 6;

        decimal newOvers = completedOvers + (currentBalls / 10m);

        bowler.overs = newOvers;

        // Team

        battingTeam.balls++;

        int teamOvers = (int)(battingTeam.balls / 6);

        int teamBalls = (int)(battingTeam.balls % 6);

        battingTeam.overs = teamOvers + (teamBalls / 10m);

    }

    private void AddRuns(

    Team battingTeam,

    Player striker,

    Player bowler,

    int runs)

    {

        battingTeam.runs += runs;

        striker.runs += runs;

        bowler.runsConceded += runs;

    }

    private void UpdateScore(Team battingTeam)

    {

        battingTeam.scores = $"{battingTeam.runs}/{battingTeam.wickets}";

    }

    private void UpdateStrikeRate(Player striker)

    {

        if (striker.balls > 0)

        {

            striker.strikeRate = Math.Round(

                (decimal)((decimal)striker.runs * 100 / striker.balls),

                2);

        }

    }

    private void UpdateEconomy(Player bowler)

    {

        if (bowler.bowlingBalls == 0)

        {

            bowler.economy = 0;

            return;

        }

        decimal totalOvers = bowler.bowlingBalls / 6m;

        bowler.economy = Math.Round(

            (decimal)(bowler.runsConceded / totalOvers),

            2

        );

    }

    private void HandleOverCompletion(

     Match match,

     Team battingTeam)

    {

        if (battingTeam.balls % 6 == 0)

        {

            _logger.LogInformation(

                "Over completed. Team: {Team}, Overs: {Overs}, Score: {Runs}/{Wickets}",

                battingTeam.shortName,

                battingTeam.overs,

                battingTeam.runs,

                battingTeam.wickets);

            SwapStrike(match);

        }

    }

    public async Task<bool> UpdateTossAsync(TossDto dto)

    {

        _logger.LogInformation(

            "Updating toss. MatchNo: {MatchNo}",

            dto.MatchNo);

        var match = await _matchRepository.GetMatchByMatchNoAsync(dto.MatchNo);

        if (match == null)

        {

            _logger.LogWarning(

                "Match not found. MatchNo: {MatchNo}",

                dto.MatchNo);

            return false;

        }

        match.tossWinner = dto.TossWinner;

        match.tossDecision = dto.TossDecision;

        _logger.LogInformation(

            "Toss updated. Winner: {Winner}, Decision: {Decision}",

            dto.TossWinner,

            dto.TossDecision);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(

            "Toss details saved successfully for MatchNo: {MatchNo}",

            dto.MatchNo);

        return true;

    }

    public async Task<bool> StartMatchAsync(int matchNo)

    {

        _logger.LogInformation(

            "Starting match. MatchNo: {MatchNo}",

            matchNo);

        // Get Match

        var match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);

        if (match == null)

        {

            _logger.LogWarning(

                "Match not found. MatchNo: {MatchNo}",

                matchNo);

            return false;

        }

        // Check Toss

        if (string.IsNullOrWhiteSpace(match.tossWinner) ||

            string.IsNullOrWhiteSpace(match.tossDecision))

        {

            _logger.LogWarning(

                "Cannot start match {MatchNo}. Toss details are not available.",

                matchNo);

            return false;

        }

        _logger.LogInformation(

            "Toss Winner: {Winner}, Decision: {Decision}",

            match.tossWinner,

            match.tossDecision);

        // Get Teams

        var teams = await _teamRepository.GetTeamsByMatchNoAsync(matchNo);

        if (teams.Count != 2)

        {

            _logger.LogWarning(

                "Unable to start Match {MatchNo}. Expected 2 teams but found {Count}.",

                matchNo,

                teams.Count);

            return false;

        }

        Team battingTeam;

        Team bowlingTeam;

        if (match.tossDecision.Equals("Bat", StringComparison.OrdinalIgnoreCase))

        {

            battingTeam = teams.First(x => x.shortName == match.tossWinner);

            bowlingTeam = teams.First(x => x.shortName != match.tossWinner);

        }

        else

        {

            bowlingTeam = teams.First(x => x.shortName == match.tossWinner);

            battingTeam = teams.First(x => x.shortName != match.tossWinner);

        }

        _logger.LogInformation(

            "Batting Team: {BattingTeam}, Bowling Team: {BowlingTeam}",

            battingTeam.shortName,

            bowlingTeam.shortName);

        // Get Players

        var battingPlayers = await _playerRepository.GetPlayersByTeamAsync(

            battingTeam.TeamId,

            matchNo);

        var bowlingPlayers = await _playerRepository.GetPlayersByTeamAsync(

            bowlingTeam.TeamId,

            matchNo);

        if (battingPlayers.Count < 2 || bowlingPlayers.Count < 1)

        {

            _logger.LogWarning(

                "Insufficient players to start Match {MatchNo}.",

                matchNo);

            return false;

        }

        var striker = battingPlayers[0];

        var nonStriker = battingPlayers[1];

        var bowler = bowlingPlayers.FirstOrDefault(x =>

            x.role == "Bowler" ||

            x.role == "All Rounder");

        if (bowler == null)

        {

            _logger.LogWarning(

                "No opening bowler available for Match {MatchNo}.",

                matchNo);

            return false;

        }

        _logger.LogInformation(

            "Opening Batters: {Striker} & {NonStriker}",

            striker.name,

            nonStriker.name);

        _logger.LogInformation(

            "Opening Bowler: {Bowler}",

            bowler.name);

        // Update Match State

        match.currentBattingTeamIndex = battingTeam.TeamId;

        match.currentBowlingTeamIndex = bowlingTeam.TeamId;

        match.strikerPlayerId = striker.playerId;

        match.nonStrikerPlayerId = nonStriker.playerId;

        match.currentBowlerPlayerId = bowler.playerId;

        match.currentInnings = 1;

        match.status = "LIVE";

        striker.status = "Batting";

        nonStriker.status = "Batting";

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(

            "Match {MatchNo} started successfully.",

            matchNo);

        return true;

    }

    public async Task<bool> ChangeBowlerAsync(ChangeBowlerDto dto)

    {

        _logger.LogInformation(

            "Changing bowler. MatchNo: {MatchNo}, NewBowlerId: {BowlerPlayerId}",

            dto.MatchNo,

            dto.BowlerPlayerId);

        var match = await _matchRepository.GetMatchByMatchNoAsync(dto.MatchNo);

        if (match == null)

        {

            _logger.LogWarning(

                "Match not found. MatchNo: {MatchNo}",

                dto.MatchNo);

            return false;

        }

        var bowlingTeam = await _teamRepository.GetTeamByIdAsync(

            match.currentBowlingTeamIndex!.Value,

            match.matchNo);

        if (bowlingTeam == null)

        {

            _logger.LogWarning(

                "Current bowling team not found. MatchNo: {MatchNo}, TeamId: {TeamId}",

                match.matchNo,

                match.currentBowlingTeamIndex);

            return false;

        }

        var bowler = await _playerRepository.GetPlayerByIdAsync(

            dto.BowlerPlayerId,

            bowlingTeam.TeamId,

            match.matchNo);

        if (bowler == null)

        {

            _logger.LogWarning(

                "Bowler not found. MatchNo: {MatchNo}, TeamId: {TeamId}, PlayerId: {PlayerId}",

                match.matchNo,

                bowlingTeam.TeamId,

                dto.BowlerPlayerId);

            return false;

        }

        if (bowler.role != "Bowler" &&

            bowler.role != "All-Rounder")

        {

            _logger.LogWarning(

                "Invalid bowler selection. Player '{PlayerName}' has role '{Role}'.",

                bowler.name,

                bowler.role);

            return false;

        }

        match.currentBowlerPlayerId = bowler.playerId;

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(

            "Bowler changed successfully. New Bowler: {BowlerName}, MatchNo: {MatchNo}",

            bowler.name,

            match.matchNo);

        return true;

    }

    private bool IsFirstInningsCompleted(Team battingTeam)

    {

        return battingTeam.overs == 20 ||

               battingTeam.wickets == 10;

    }

    private bool IsMatchCompleted(

      Match match,

      Team battingTeam,

      Team bowlingTeam)

    {

        if (match.currentInnings != 2)

            return false;

        int target = (int)(bowlingTeam.runs + 1);

        return battingTeam.runs >= target ||

               battingTeam.wickets >= 10 ||

               battingTeam.overs >= 20;

    }

    private async Task FinishMatch(

     Match match,

     Team battingTeam,

     Team bowlingTeam)

    {

        battingTeam.totalMatch++;

        bowlingTeam.totalMatch++;

        if (battingTeam.runs > bowlingTeam.runs)

        {

            int wicketsLeft = (int)(10 - battingTeam.wickets);

            match.result =

                $"{battingTeam.shortName} won by {wicketsLeft} wickets";

            battingTeam.winCount++;

            bowlingTeam.lossCount++;

        }

        else if (battingTeam.runs < bowlingTeam.runs)

        {

            int runsMargin = (int)(bowlingTeam.runs - battingTeam.runs);

            match.result =

                $"{bowlingTeam.shortName} won by {runsMargin} runs";

            bowlingTeam.winCount++;

            battingTeam.lossCount++;

        }

        else

        {

            match.result = "Match Tied";

        }

    }

    private async Task UpdateUpcomingMatchesStatsAsync(

    Team liveBattingTeam,

    Team liveBowlingTeam)

    {

        var upcomingMatches = await _matchRepository.GetUpcomingMatchesAsync();

        foreach (var upcomingMatch in upcomingMatches)

        {

            var teams = await _teamRepository.GetTeamsByMatchNoAsync(upcomingMatch.matchNo);

            foreach (var team in teams)

            {

                if (team.shortName == liveBattingTeam.shortName)

                {

                    team.winCount = liveBattingTeam.winCount;

                    team.lossCount = liveBattingTeam.lossCount;

                    team.totalMatch = liveBattingTeam.totalMatch;

                    team.matchStatus = liveBattingTeam.matchStatus;

                }

                if (team.shortName == liveBowlingTeam.shortName)

                {

                    team.winCount = liveBowlingTeam.winCount;

                    team.lossCount = liveBowlingTeam.lossCount;

                    team.totalMatch = liveBowlingTeam.totalMatch;

                    team.matchStatus = liveBowlingTeam.matchStatus;

                }

            }

        }

        // Save all updates only once

        await _unitOfWork.SaveChangesAsync();

    }

    public async Task<bool> UpdatePlayerOfTheMatchAsync(

     int matchNo,

     PlayerOfTheMatchDto dto)

    {

        _logger.LogInformation(

            "Updating Player of the Match. MatchNo: {MatchNo}, PlayerId: {PlayerId}",

            matchNo,

            dto.PlayerId);

        var match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);

        if (match == null)

        {

            _logger.LogWarning(

                "Match not found. MatchNo: {MatchNo}",

                matchNo);

            return false;

        }

        var players = await _playerRepository.GetPlayersByMatchNoAsync(matchNo);

        var selectedPlayer = players.FirstOrDefault(

            x => x.playerId == dto.PlayerId);

        if (selectedPlayer == null)

        {

            _logger.LogWarning(

                "Player not found. MatchNo: {MatchNo}, PlayerId: {PlayerId}",

                matchNo,

                dto.PlayerId);

            return false;

        }

        match.playerOfTheMatch = selectedPlayer.name;

        _logger.LogInformation(

            "Player of the Match selected: {PlayerName}",

            selectedPlayer.name);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(

            "Player of the Match updated successfully for MatchNo: {MatchNo}",

            matchNo);

        return true;

    }

    public async Task<bool> StartSecondInningsAsync(int matchNo)

    {

        var match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);

        if (match == null)

            return false;

        if (match.currentInnings == 2)

            return true;

        var battingTeam =

            await _teamRepository.GetTeamByIdAsync(

                match.currentBattingTeamIndex!.Value,

                match.matchNo);

        var bowlingTeam =

            await _teamRepository.GetTeamByIdAsync(

                match.currentBowlingTeamIndex!.Value,

                match.matchNo);

        if (battingTeam == null || bowlingTeam == null)

            return false;

        // swap innings

        match.currentInnings = 2;

        var temp = match.currentBattingTeamIndex;

        match.currentBattingTeamIndex =

            match.currentBowlingTeamIndex;

        match.currentBowlingTeamIndex =

            temp;

        // reset striker/non striker/current bowler

        // New batting team (after swap)

        var secondInningsBattingTeam =

            await _teamRepository.GetTeamByIdAsync(

                match.currentBattingTeamIndex!.Value,

                match.matchNo);

        var secondInningsBowlingTeam =

            await _teamRepository.GetTeamByIdAsync(

                match.currentBowlingTeamIndex!.Value,

                match.matchNo);

        // Opening batters

        var openers = await _playerRepository.GetPlayersByTeamAsync(

            secondInningsBattingTeam!.TeamId,

            match.matchNo);

        match.strikerPlayerId = openers[0].playerId;

        match.nonStrikerPlayerId = openers[1].playerId;

        openers[0].status = "Batting";

        openers[1].status = "Batting";

        // Opening bowler

        var bowlers = await _playerRepository.GetPlayersByTeamAsync(

            secondInningsBowlingTeam!.TeamId,

            match.matchNo);

        var openingBowler = bowlers.First(p => p.role == "Bowler");

        match.currentBowlerPlayerId = openingBowler.playerId;

        await _matchRepository.UpdateMatchAsync(match);

        return true;

    }

    private void StoreBall(int matchNo, int innings, string ball)

    {

        if (innings == 1)

        {

            if (!MatchBallStore.FirstInningsBalls.TryGetValue(matchNo, out var balls))

            {

                balls = new List<string>();

                MatchBallStore.FirstInningsBalls[matchNo] = balls;

            }

            balls.Add(ball);

        }

        else

        {

            if (!MatchBallStore.SecondInningsBalls.TryGetValue(matchNo, out var balls))

            {

                balls = new List<string>();

                MatchBallStore.SecondInningsBalls[matchNo] = balls;

            }

            balls.Add(ball);

        }

    }

    public async Task<bool> CompleteMatchAsync(CompletedMatchDto dto)
    {
        // Get Match
        var match = await _matchRepository.GetMatchByMatchNoAsync(dto.MatchNo);

        if (match == null)
            return false;

        // Get Players
        var players = await _playerRepository.GetPlayersByMatchNoAsync(dto.MatchNo);

        // Batting - All Players
        var battingList = _mapper.Map<List<Batting>>(players);

        // Bowling - Only Bowlers & All-Rounders
        var bowlingPlayers = players
            .Where(p => p.role == "Bowler" || p.role == "All-Rounder")
            .ToList();

        var bowlingList = _mapper.Map<List<Bowling>>(bowlingPlayers);

        // Save Batting
        await _battingRepository.AddRangeAsync(battingList);

        // Save Bowling
        await _bowlingRepository.AddRangeAsync(bowlingList);

        // Delete Players
        await _playerRepository.DeletePlayersByMatchNoAsync(dto.MatchNo);

        // Update Team Statistics
        var teams = await _teamRepository.GetTeamsByMatchNoAsync(dto.MatchNo);

        if (teams.Count == 2)
        {
            var team1 = teams[0];
            var team2 = teams[1];

            if (team1.runs > team2.runs)
            {
                UpdateTeamStatistics(team1, true);
                UpdateTeamStatistics(team2, false);
            }
            else if (team2.runs > team1.runs)
            {
                UpdateTeamStatistics(team1, false);
                UpdateTeamStatistics(team2, true);
            }
            await UpdateUpcomingMatchesStatsAsync(team1, team2);
        }

        // Update Match
        match.playerOfTheMatch = dto.PlayerOfTheMatch;
        match.status = "COMPLETED";

        // Save Everything
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> PromoteUpcomingMatchAsync(int matchNo)

    {

        _logger.LogInformation(

            "Promoting upcoming match to LIVE. MatchNo: {MatchNo}",

            matchNo);

        var match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);

        if (match == null)

        {

            _logger.LogWarning(

                "Unable to promote match. Match not found. MatchNo: {MatchNo}",

                matchNo);

            return false;

        }

        match.status = "LIVE";

        _logger.LogInformation(

            "Match status updated to LIVE. MatchNo: {MatchNo}",

            matchNo);

        MatchBallStore.FirstInningsBalls.TryRemove(matchNo, out _);

        MatchBallStore.SecondInningsBalls.TryRemove(matchNo, out _);

        _logger.LogInformation(

            "Cleared ball history for MatchNo: {MatchNo}",

            matchNo);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(

            "Upcoming match promoted successfully. MatchNo: {MatchNo}",

            matchNo);

        return true;

    }

    public async Task<List<MatchDto>> GetUpcomingMatchesAsync()

    {

        _logger.LogInformation(

            "Fetching upcoming matches.");

        var matches = await _matchRepository.GetUpcomingMatchesAsync();

        if (matches.Count == 0)

        {

            _logger.LogWarning(

                "No upcoming matches found.");

            return new List<MatchDto>();

        }

        _logger.LogInformation(

            "Retrieved {MatchCount} upcoming match(es).",

            matches.Count);

        var result = new List<MatchDto>();

        foreach (var match in matches)

        {

            _logger.LogInformation(

                "Loading MatchNo: {MatchNo}",

                match.matchNo);

            var teams = await _teamRepository.GetTeamsByMatchNoAsync(match.matchNo);

            var players = await _playerRepository.GetPlayersByMatchNoAsync(match.matchNo);

            _logger.LogInformation(

                "Retrieved {TeamCount} teams and {PlayerCount} players for MatchNo: {MatchNo}.",

                teams.Count,

                players.Count,

                match.matchNo);

            var matchDto = _mapper.Map<MatchDto>(match);

            foreach (var team in teams)

            {

                var teamDto = _mapper.Map<TeamDto>(team);

                teamDto.Players = _mapper.Map<List<PlayerDto>>(

                    players

                        .Where(p => p.TeamId == team.TeamId)

                        .ToList());

                matchDto.Teams.Add(teamDto);

                _logger.LogInformation(

                    "Mapped Team {TeamName} with {PlayerCount} players.",

                    team.shortName,

                    teamDto.Players.Count);

            }

            matchDto.FirstInningsBalls =

                MatchBallStore.FirstInningsBalls.GetValueOrDefault(

                    match.matchNo,

                    new List<string>());

            matchDto.SecondInningsBalls =

                MatchBallStore.SecondInningsBalls.GetValueOrDefault(

                    match.matchNo,

                    new List<string>());

            _logger.LogInformation(

                "Loaded ball history for MatchNo: {MatchNo}. First Innings: {FirstCount}, Second Innings: {SecondCount}",

                match.matchNo,

                matchDto.FirstInningsBalls.Count,

                matchDto.SecondInningsBalls.Count);

            result.Add(matchDto);

        }

        _logger.LogInformation(

            "Successfully prepared {MatchCount} upcoming match(es).",

            result.Count);

        return result;

    }
    private void UpdateTeamStatistics(Team team, bool isWinner)
    {
        // Update last 5 match status
        var statusList = string.IsNullOrWhiteSpace(team.matchStatus)
            ? new List<string>()
            : team.matchStatus.Split(',').ToList();

        statusList.Add(isWinner ? "true" : "false");

        // Keep only last 5 matches
        if (statusList.Count > 5)
        {
            statusList.RemoveAt(0);
        }

        team.matchStatus = string.Join(",", statusList);
    }

}
