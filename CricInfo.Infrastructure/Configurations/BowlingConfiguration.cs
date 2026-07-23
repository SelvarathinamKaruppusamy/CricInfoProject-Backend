using CricInfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CricInfo.Infrastructure.Configurations
{
    public class BowlingConfiguration : IEntityTypeConfiguration<Bowling>
    {
        public void Configure(EntityTypeBuilder<Bowling> builder)
        {
            builder.ToTable("Bowling");

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

            builder.Property(x => x.overs)
                   .HasMaxLength(255);

            builder.Property(x => x.economy)
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