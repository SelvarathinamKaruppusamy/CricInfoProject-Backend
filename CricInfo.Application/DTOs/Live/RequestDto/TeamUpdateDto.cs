namespace CricInfo.Application.DTOs.Live.RequestDto;
public class TeamUpdateDto
{
    public string? scores { get; set; }

    public int runs { get; set; }

    public int wickets { get; set; }

    public int extras { get; set; }

    public decimal overs { get; set; }

    public int balls { get; set; }

    public int winCount { get; set; }

    public int lossCount { get; set; }

    public int totalMatch { get; set; }

    public string? matchStatus { get; set; }
}