namespace CricInfo.Application.DTOs.Live.RequestDto;
public class PlayerUpdateDto
{
    public int runs { get; set; }

    public int balls { get; set; }

    public int fours { get; set; }

    public int sixes { get; set; }

    public decimal strikeRate { get; set; }

    public string? status { get; set; }

    public decimal overs { get; set; }

    public int wickets { get; set; }

    public int maidens { get; set; }

    public int runsConceded { get; set; }

    public decimal economy { get; set; }
}