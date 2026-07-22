using CricInfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CricInfo.Infrastructure.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");

        builder.HasKey(x => new
        {
            x.TeamId,
            x.matchNo,
            x.playerId
        });

        builder.Property(x => x.id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.playerId)
               .ValueGeneratedNever();

        builder.Property(x => x.TeamId)
               .ValueGeneratedNever();

        builder.Property(x => x.matchNo)
               .ValueGeneratedNever();

        builder.Property(x => x.name)
               .HasMaxLength(255);

        builder.Property(x => x.role)
               .HasMaxLength(255);

        builder.Property(x => x.status)
               .HasMaxLength(255);

        builder.HasOne(x => x.Team)
               .WithMany(x => x.Players)
               .HasForeignKey(x => new
               {
                   x.TeamId,
                   x.matchNo
               })
               .HasPrincipalKey(x => new
               {
                   x.TeamId,
                   x.matchNo
               });
    }
}