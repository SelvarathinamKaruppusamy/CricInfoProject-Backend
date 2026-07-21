namespace CricInfo.Application.DTOs.CompletedModule;

public class CompletedTeamResponseDto
{
    public int teamId { get; set; }

    public string? fullName { get; set; }

    public string? shortName { get; set; }

    public string? logo { get; set; }

    public string? scores { get; set; }

    public int? runs { get; set; }

    public int? wickets { get; set; }

    public int? extras { get; set; }

    public decimal? overs { get; set; }

    public int? balls { get; set; }

    public int? winCount { get; set; }

    public int? lossCount { get; set; }

    public int? totalMatch { get; set; }

    public string? matchStatus { get; set; }

    public List<BattingResponseDto> batting { get; set; } = new();

    public List<BowlingResponseDto> bowling { get; set; } = new();
}