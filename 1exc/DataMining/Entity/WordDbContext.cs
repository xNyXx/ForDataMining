using System;
using System.Data.SQLite;
using System.Data.SQLite.EF6;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;

namespace DataMining.Entity
{
    public class WordDbContext : DbContext
    {
        public DbSet<Word> Words { get; set; }

        public WordDbContext() : base()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                modelBuilder.Entity<Word>().HasKey(w => w.Text);
                modelBuilder.Entity<Word>().Property(w => w.Count).HasDefaultValue(1);
                modelBuilder.Entity<Word>().Property(w => w.Id).ValueGeneratedOnAdd();
                modelBuilder.Entity<Word>().Property(w => w.Id).HasDefaultValue(1);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = app_words.db");
        }                                
    }
}