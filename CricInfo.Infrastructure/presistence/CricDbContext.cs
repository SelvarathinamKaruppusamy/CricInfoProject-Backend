using CricInfo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


namespace CricInfo.Infrastructure.presistence
{
    public class CricDbContext : DbContext
    {
        public CricDbContext(DbContextOptions<CricDbContext> options)
        : base(options)
        {
        }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CricDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
