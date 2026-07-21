using CricInfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CricInfo.Infrastructure.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("Teams");

        builder.HasKey(x => new
        {
            x.TeamId,
            x.matchNo
        });

        builder.Property(x => x.id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.TeamId)
               .ValueGeneratedNever();

        builder.Property(x => x.matchNo)
               .ValueGeneratedNever();

        builder.Property(x => x.fullName)
               .HasMaxLength(255);

        builder.Property(x => x.shortName)
               .HasMaxLength(255);

        builder.Property(x => x.logo)
               .HasMaxLength(255);

        builder.Property(x => x.scores)
               .HasMaxLength(255);

        builder.Property(x => x.matchStatus)
               .HasMaxLength(255);

        builder.HasOne(x => x.Match)
               .WithMany(x => x.Teams)
               .HasForeignKey(x => x.matchNo)
               .HasPrincipalKey(x => x.matchNo);
    }
}