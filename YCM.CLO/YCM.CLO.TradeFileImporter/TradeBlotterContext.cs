using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace YCM.CLO.TradeFileImporter
{
    public class TradeBlotterContext : DbContext
    {
        public DbSet<TradeBlotterJob> TradeBlotterJobs { get; set; }
        public DbSet<StageTradeBlotter> StageTradeBlotters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                Program.Configuration.GetConnectionString("TradeBlotterDataBase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StageTradeBlotter>()
                .HasKey(c => new { c.TradeBlotterJobId, c.Trade_ID });
        }
    }
}
