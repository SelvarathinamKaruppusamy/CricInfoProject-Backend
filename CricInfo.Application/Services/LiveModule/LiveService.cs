using AutoMapper;
using CricInfo.API.Memory;
using CricInfo.Application.DTOs.Live.RequestDto;
using CricInfo.Application.DTOs.Live.ResponseDto;
using CricInfo.Application.Interfaces.Repositories.LiveModule;
using CricInfo.Application.Interfaces.Services.LiveModule;
using CricInfo.Domain.Entities;

namespace CricInfo.Application.Services.LiveModule;

public class LiveService : ILiveService
{
    private readonly IMatchRepository _matchRepository;
    private readonly ITeamRepository _teamRepository;
    private readonly IPlayerRepository _playerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;



    public LiveService(
        IMatchRepository matchRepository,
        ITeamRepository teamRepository,
        IPlayerRepository playerRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _matchRepository = matchRepository;
        _teamRepository = teamRepository;
        _playerRepository = playerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<MatchDto?> GetLiveMatchAsync()
    {
        var match = await _matchRepository.GetCurrentLiveMatchAsync();

        if (match == null)
            return null;

        var matchNo = match.matchNo;

        var teams = await _teamRepository.GetTeamsByMatchNoAsync(matchNo);

        var players = await _playerRepository.GetPlayersByMatchNoAsync(matchNo);

        var matchDto = _mapper.Map<MatchDto>(match);

        foreach (var team in teams)
        {
            var teamDto = _mapper.Map<TeamDto>(team);

            teamDto.Players = _mapper.Map<List<PlayerDto>>(
                players.Where(p => p.TeamId == team.TeamId).ToList()
            );

            matchDto.Teams.Add(teamDto);
        }

        matchDto.FirstInningsBalls =
            MatchBallStore.FirstInningsBalls.GetValueOrDefault(
                matchNo,
                new List<string>());

        matchDto.SecondInningsBalls =
            MatchBallStore.SecondInningsBalls.GetValueOrDefault(
                matchNo,
                new List<string>());

        return matchDto;
    }

    public async Task<PlayerDto?> GetLivePlayerAsync(int playerid, int teamId, int matchNo)
    {
        var Player = await _playerRepository.GetPlayerByIdAsync(playerid, teamId, matchNo);
        return _mapper.Map<PlayerDto>(Player);
    }

    public async Task<TeamDto?> GetLiveTeamAsync(int teamid, int MatchNo)
    {
        var Team = await _teamRepository.GetTeamByIdAsync(teamid, MatchNo);
        return _mapper.Map<TeamDto?>(Team);
    }

    public async Task<bool> UpdateMatchAsync(int matchNo, MatchUpdateDto dto)
    {
        var Match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);
        if (Match == null)
            return false;
        _mapper.Map(dto, Match);
        await _matchRepository.UpdateMatchAsync(Match);
        return true;
    }

    public async Task<bool> UpdatePlayerAsync(int playerId, int teamId, int matchNo, PlayerUpdateDto dto)
    {
        var Player = await _playerRepository.GetPlayerByIdAsync(playerId, teamId, matchNo);
        if (Player == null) return false;
        _mapper.Map(dto, Player);
        await _playerRepository.UpdatePlayerAsync(Player);
        return true;
    }

    public async Task<bool> UpdateTeamAsync(int teamId, int matchNo, TeamUpdateDto dto)
    {
        var Team = await _teamRepository.GetTeamByIdAsync(teamId, matchNo);
        if (Team == null) return false;
        _mapper.Map(dto, Team);
        await _teamRepository.UpdateTeamAsync(Team);
        return true;
    }
    public async Task<bool> ProcessBallAsync(BallUpdateDto dto)
    {
        var match = await _matchRepository.GetMatchByMatchNoAsync(dto.matchNo);

        if (match == null)
            return false;

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
                UpdateLegalBall(battingTeam, striker, bowler);
                UpdateStrikeRate(striker);
                UpdateEconomy(bowler);
                HandleOverCompletion(match, battingTeam);
                break;

            case "1":
                UpdateLegalBall(battingTeam, striker, bowler);
                AddRuns(battingTeam, striker, bowler, 1);
                UpdateScore(battingTeam);
                UpdateStrikeRate(striker);
                UpdateEconomy(bowler);
                SwapStrike(match);
                HandleOverCompletion(match, battingTeam);
                break;

            case "2":
                UpdateLegalBall(battingTeam, striker, bowler);
                AddRuns(battingTeam, striker, bowler, 2);
                UpdateScore(battingTeam);
                UpdateStrikeRate(striker);
                UpdateEconomy(bowler);
                HandleOverCompletion(match, battingTeam);
                break;

            case "3":
                UpdateLegalBall(battingTeam, striker, bowler);
                AddRuns(battingTeam, striker, bowler, 3);
                UpdateScore(battingTeam);
                UpdateStrikeRate(striker);
                UpdateEconomy(bowler);
                SwapStrike(match);
                HandleOverCompletion(match, battingTeam);
                break;

            case "4":
                UpdateLegalBall(battingTeam, striker, bowler);
                AddRuns(battingTeam, striker, bowler, 4);
                striker.fours++;
                UpdateScore(battingTeam);
                UpdateStrikeRate(striker);
                UpdateEconomy(bowler);
                HandleOverCompletion(match, battingTeam);
                break;

            case "6":
                UpdateLegalBall(battingTeam, striker, bowler);
                AddRuns(battingTeam, striker, bowler, 6);
                striker.sixes++;
                UpdateScore(battingTeam);
                UpdateStrikeRate(striker);
                UpdateEconomy(bowler);
                HandleOverCompletion(match, battingTeam);
                break;

            case "WD":
                HandleWide(battingTeam, bowler);
                break;

            case "NB":
                HandleNoBall(battingTeam, bowler);
                break;

            case "W":
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
            await StartSecondInningsAsync(match.matchNo);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        if (match.currentInnings == 2 &&
    IsMatchCompleted(match, battingTeam, bowlingTeam))
        {
            FinishMatch(match, battingTeam, bowlingTeam);
            MatchBallStore.FirstInningsBalls.TryRemove(match.matchNo, out _);
            MatchBallStore.SecondInningsBalls.TryRemove(match.matchNo, out _);
        }
        StoreBall(
    match.matchNo,
    match.currentInnings!.Value,
    dto.ballResult
);
        await _unitOfWork.SaveChangesAsync();

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
    }
    private async Task HandleWicket(
     Team battingTeam,
     Player striker,
     Player bowler,
     Match match)
    {
        battingTeam.wickets++;

        striker.status = "Out";

        bowler.wickets++;

        var nextBatter = await _playerRepository.GetNextBatterAsync(
            battingTeam.TeamId,
            match.matchNo);

        if (nextBatter == null)
        {
            UpdateScore(battingTeam);
            return;
        }

        nextBatter.status = "Batting";

        // Keep the non-striker batting
        var nonStriker = await _playerRepository.GetPlayerByIdAsync(
            match.nonStrikerPlayerId!.Value,
            battingTeam.TeamId,
            match.matchNo);

        if (nonStriker != null)
        {
            nonStriker.status = "Batting";
        }

        match.strikerPlayerId = nextBatter.playerId;

        UpdateScore(battingTeam);
    }
    private static void SwapStrike(Match match)
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
            //battingTeam.overs++;
            SwapStrike(match);
        }
    }
    public async Task<bool> UpdateTossAsync(TossDto dto)
    {
        var match = await _matchRepository.GetMatchByMatchNoAsync(dto.MatchNo);

        if (match == null)
            return false;

        match.tossWinner = dto.TossWinner;
        match.tossDecision = dto.TossDecision;

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    public async Task<bool> StartMatchAsync(int matchNo)
    {
        // Get Match
        var match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);

        if (match == null)
            return false;

        // Check Toss
        if (string.IsNullOrWhiteSpace(match.tossWinner) ||
            string.IsNullOrWhiteSpace(match.tossDecision))
            return false;

        // Get Teams
        var teams = await _teamRepository.GetTeamsByMatchNoAsync(matchNo);

        if (teams.Count != 2)
            return false;

        Team battingTeam;
        Team bowlingTeam;

        // Decide Batting & Bowling Team
        if (match.tossDecision.Equals("Bat", StringComparison.OrdinalIgnoreCase))
        {
            battingTeam = teams.FirstOrDefault(x => x.shortName == match.tossWinner)!;
            bowlingTeam = teams.FirstOrDefault(x => x.shortName != match.tossWinner)!;
        }
        else
        {
            bowlingTeam = teams.FirstOrDefault(x => x.shortName == match.tossWinner)!;
            battingTeam = teams.FirstOrDefault(x => x.shortName != match.tossWinner)!;
        }

        if (battingTeam == null || bowlingTeam == null)
            return false;

        // Get Players
        var battingPlayers = await _playerRepository.GetPlayersByTeamAsync(
            battingTeam.TeamId,
            matchNo);

        var bowlingPlayers = await _playerRepository.GetPlayersByTeamAsync(
            bowlingTeam.TeamId,
            matchNo);

        if (battingPlayers.Count < 2 || bowlingPlayers.Count < 1)
            return false;

        // Select Opening Batters
        var striker = battingPlayers[0];
        var nonStriker = battingPlayers[1];

        // Select Opening Bowler (Bowler or All Rounder)
        var bowler = bowlingPlayers.FirstOrDefault(x =>
            x.role == "Bowler" ||
            x.role == "All Rounder");

        if (bowler == null)
            return false;

        // Update Match State
        match.currentBattingTeamIndex = battingTeam.TeamId;
        match.currentBowlingTeamIndex = bowlingTeam.TeamId;

        match.strikerPlayerId = striker.playerId;
        match.nonStrikerPlayerId = nonStriker.playerId;

        match.currentBowlerPlayerId = bowler.playerId;

        match.currentInnings = 1;
        match.status = "LIVE";

        // Update Player Status
        striker.status = "Batting";
        nonStriker.status = "Batting";

        // Save
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ChangeBowlerAsync(ChangeBowlerDto dto)
    {
        var match = await _matchRepository.GetMatchByMatchNoAsync(dto.MatchNo);

        if (match == null)
        {
            Console.WriteLine("Match is null");
            return false;
        }


        var bowlingTeam = await _teamRepository.GetTeamByIdAsync(
            match.currentBowlingTeamIndex!.Value,
            match.matchNo);

        if (bowlingTeam == null)
        {
            Console.WriteLine("BowlingTeam is null");
            return false;
        }
        var bowler = await _playerRepository.GetPlayerByIdAsync(
            dto.BowlerPlayerId,
            bowlingTeam.TeamId,
            match.matchNo);

        if (bowler == null)
        {
            Console.WriteLine("Bowler is null");
            return false;
        }

        if (bowler.role != "Bowler" &&
            bowler.role != "All-Rounder")
        {
            Console.WriteLine("Bowlerand All Rounter is null");
            return false;
        }

        match.currentBowlerPlayerId = bowler.playerId;

        await _unitOfWork.SaveChangesAsync();

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
    private void FinishMatch(
     Match match,
     Team battingTeam,
     Team bowlingTeam)
    {
        //match.status = "COMPLETED";

        // Chasing team wins
        if (battingTeam.runs > bowlingTeam.runs)
        {
            int wicketsLeft = (int)(10 - battingTeam.wickets);

            match.result =
                $"{battingTeam.shortName} won by {wicketsLeft} wickets";

            return;
        }

        // Defending team wins
        if (battingTeam.runs < bowlingTeam.runs)
        {
            int runsMargin = (int)(bowlingTeam.runs - battingTeam.runs);

            match.result =
                $"{bowlingTeam.shortName} won by {runsMargin} runs";

            return;
        }

        match.result = "Match Tied";
    }
    public async Task<bool> UpdatePlayerOfTheMatchAsync(
    int matchNo,
    PlayerOfTheMatchDto dto)
    {
        var match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);

        if (match == null)
            return false;

        var player = await _playerRepository.GetPlayersByMatchNoAsync(matchNo);

        var selectedPlayer = player.FirstOrDefault(x => x.playerId == dto.PlayerId);

        if (selectedPlayer == null)
            return false;

        match.playerOfTheMatch = selectedPlayer.name;

        await _unitOfWork.SaveChangesAsync();

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
        var match = await _matchRepository.GetMatchByMatchNoAsync(dto.MatchNo);

        if (match == null)
            return false;

        // Update Player of the Match
        match.playerOfTheMatch = dto.PlayerOfTheMatch;

        // Mark current match as completed
        match.status = "COMPLETED";

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    public async Task<bool> PromoteUpcomingMatchAsync(int matchNo)
    {
        var match = await _matchRepository.GetMatchByMatchNoAsync(matchNo);

        if (match == null)
            return false;

        match.status = "LIVE";

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    public async Task<List<MatchDto>> GetUpcomingMatchesAsync()
    {
        var matches = await _matchRepository.GetUpcomingMatchesAsync();

        var result = new List<MatchDto>();

        foreach (var match in matches)
        {
            var teams = await _teamRepository.GetTeamsByMatchNoAsync(match.matchNo);

            var players = await _playerRepository.GetPlayersByMatchNoAsync(match.matchNo);

            var matchDto = _mapper.Map<MatchDto>(match);

            foreach (var team in teams)
            {
                var teamDto = _mapper.Map<TeamDto>(team);

                teamDto.Players = _mapper.Map<List<PlayerDto>>(
                    players
                        .Where(p => p.TeamId == team.TeamId)
                        .ToList()
                );

                matchDto.Teams.Add(teamDto);
            }

            matchDto.FirstInningsBalls =
                MatchBallStore.FirstInningsBalls.GetValueOrDefault(
                    match.matchNo,
                    new List<string>()
                );

            matchDto.SecondInningsBalls =
                MatchBallStore.SecondInningsBalls.GetValueOrDefault(
                    match.matchNo,
                    new List<string>()
                );

            result.Add(matchDto);
        }

        return result;
    }
}