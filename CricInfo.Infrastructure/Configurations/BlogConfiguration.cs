using CricInfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CricInfo.Infrastructure.Configurations;

public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        // Table Name
        builder.ToTable("Blogs");

        // Primary Key
        builder.HasKey(b => b.Id);

        // Columns
        builder.Property(b => b.Id)
               .HasColumnName("id");

        builder.Property(b => b.MatchNo)
               .HasColumnName("matchNo")
               .IsRequired();

        builder.Property(b => b.Title)
               .HasColumnName("title")
               .HasMaxLength(500)
               .IsRequired();

        builder.Property(b => b.Slug)
               .HasColumnName("slug")
               .HasMaxLength(555)
               .IsRequired();

        builder.HasIndex(b => b.Slug)
               .IsUnique();

        builder.Property(b => b.Image)
               .HasColumnName("image")
               .HasMaxLength(500);

        builder.Property(b => b.ShortDescription)
               .HasColumnName("shortDescription");

        builder.Property(b => b.Category)
               .HasColumnName("category")
               .HasMaxLength(100);

        builder.Property(b => b.Author)
               .HasColumnName("author")
               .HasMaxLength(100);

        builder.Property(b => b.Content)
               .HasColumnName("content");

        builder.Property(b => b.PublishedDate)
               .HasColumnName("publishedDate");

        builder.Property(b => b.ReadTime)
               .HasColumnName("readTime")
               .HasMaxLength(20);

        builder.Property(b => b.Featured)
               .HasColumnName("featured")
               .HasDefaultValue(false);

        builder.Property(b => b.Tags)
               .HasColumnName("tags");
    }
}