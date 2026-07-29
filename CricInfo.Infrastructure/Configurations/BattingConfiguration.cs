using CricInfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CricInfo.Infrastructure.Configurations
{
    public class BattingConfiguration : IEntityTypeConfiguration<Batting>
    {
        public void Configure(EntityTypeBuilder<Batting> builder)
        {
            builder.ToTable("Batting");

            builder.HasKey(x => new
            {
                x.TeamId,
                x.MatchNo,
                x.PlayerId
            });

            builder.Property(x => x.id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.name)
                   .HasMaxLength(255);

            builder.Property(x => x.role)
                   .HasMaxLength(255);

            builder.Property(x => x.status)
                   .HasMaxLength(255);

            builder.Property(x => x.strikeRate)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne<Team>()
                   .WithMany()
                   .HasForeignKey(x => new
                   {
                       x.TeamId,
                       x.MatchNo
                   });
        }
    }
}