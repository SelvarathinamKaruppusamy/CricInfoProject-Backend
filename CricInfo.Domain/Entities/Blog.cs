using System.ComponentModel.DataAnnotations;

namespace CricInfo.Domain.Entities;

public class Blog
{
    [Key]
    public int Id { get; set; }

    public int MatchNo { get; set; }

    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(555)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Image { get; set; }

    public string? ShortDescription { get; set; }

    [MaxLength(100)]
    public string? Category { get; set; }

    [MaxLength(100)]
    public string? Author { get; set; }

    public string? Content { get; set; }

    public DateTime PublishedDate { get; set; }

    [MaxLength(20)]
    public string? ReadTime { get; set; }

    public bool Featured { get; set; }

    public string? Tags { get; set; }
}