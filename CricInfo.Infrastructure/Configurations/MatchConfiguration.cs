using CricInfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CricInfo.Infrastructure.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");

        builder.HasKey(x => x.matchNo);

        builder.Property(x => x.id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.matchNo)
               .ValueGeneratedNever();

        builder.Property(x => x.venue)
               .HasMaxLength(255);

        builder.Property(x => x.city)
               .HasMaxLength(255);

        builder.Property(x => x.tossWinner)
               .HasMaxLength(255);

        builder.Property(x => x.tossDecision)
               .HasMaxLength(255);

        builder.Property(x => x.result)
               .HasMaxLength(255);

        builder.Property(x => x.playerOfTheMatch)
               .HasMaxLength(255);

        builder.Property(x => x.status)
               .HasMaxLength(255);
    }
}