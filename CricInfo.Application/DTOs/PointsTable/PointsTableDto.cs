namespace CricInfo.Application.DTOs.PointsTable;

public class PointsTableDto
{
    public int TeamId { get; set; }

    public string? TeamName { get; set; }

    public string? ShortName { get; set; }

    public string? Logo { get; set; }

    public int Played { get; set; }

    public int Wins { get; set; }

    public int Losses { get; set; }

    public int Points { get; set; }

    public decimal NRR { get; set; }

    public List<bool> Form { get; set; } = new();
}