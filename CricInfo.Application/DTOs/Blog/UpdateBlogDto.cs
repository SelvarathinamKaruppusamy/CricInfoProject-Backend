using System.ComponentModel.DataAnnotations;

namespace CricInfo.Application.DTOs.Blog;

public class UpdateBlogDto
{
    [Required]
    public int MatchId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Slug { get; set; } = string.Empty;
    
    public string? Image { get; set; }

    public string? ShortDescription { get; set; }

    public string? Category { get; set; }

    public string? Author { get; set; }

    public List<string> Content { get; set; } = [];

    public DateTime PublishedDate { get; set; }

    public string? ReadTime { get; set; }

    public bool Featured { get; set; }

    public List<string> Tags { get; set; } = [];
}