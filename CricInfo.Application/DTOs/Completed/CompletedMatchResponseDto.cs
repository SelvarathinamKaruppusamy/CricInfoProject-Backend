namespace CricInfo.Application.DTOs.CompletedModule;

public class CompletedMatchResponseDto
{
    public int matchNo { get; set; }

    public string? venue { get; set; }

    public string? city { get; set; }

    public DateOnly? date { get; set; }

    public string? tossWinner { get; set; }

    public string? tossDecision { get; set; }

    public string? result { get; set; }

    public string? playerOfTheMatch { get; set; }

    public string? status { get; set; }

    public List<CompletedTeamResponseDto> teams { get; set; } = new();
}