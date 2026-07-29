namespace CricInfo.Application.DTOs.Blog;

public class BlogDto
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string? Image { get; set; }

    public string? ShortDescription { get; set; }

    public string? Category { get; set; }

    public string? Author { get; set; }

    public List<string> Content { get; set; } = [];

    public string PublishedDate { get; set; } = string.Empty;

    public string? ReadTime { get; set; }

    public bool Featured { get; set; }

    public List<string> Tags { get; set; } = [];
}