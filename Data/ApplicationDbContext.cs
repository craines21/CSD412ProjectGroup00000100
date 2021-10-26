using System;
using System.Collections.Generic;
using System.Text;
using CSD412ProjectGroup00000100.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSD412ProjectGroup00000100.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // creates and/or updates the data base to the latest migration
            this.Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Poll>()
            .HasOne(p => p.User)
            .WithMany(c => c.Polls)
            .HasForeignKey(p =>p.UserId);

            builder.Entity<Poll>().HasKey(p => new {p.PollId});

            builder.Entity<Poll>().Property(f => f.PollId)
            .ValueGeneratedOnAdd();

            builder.Entity<Item>()
            .HasOne(p => p.Poll)
            .WithMany(c => c.Items)
            .IsRequired()
            .HasForeignKey(p => new {p.PollId});

            builder.Entity<Item>().HasKey(p => new {p.ItemId});

            builder.Entity<Item>().Property(f => f.ItemId)
            .ValueGeneratedOnAdd();

            builder.Entity<Vote>()
            .HasOne(p => p.Item)
            .WithMany(c => c.Votes)
            .IsRequired()
            .HasForeignKey(p => new { p.ItemId });

            builder.Entity<Vote>()
            .HasOne(p => p.Voter)
            .WithMany(c => c.Votes)
            .IsRequired()
            .HasForeignKey(p =>  p.VoterId );

            builder.Entity<Vote>().HasKey(p => new {p.VoteId });

            builder.Entity<Vote>().Property(f => f.VoteId)
            .ValueGeneratedOnAdd();
        }
        public DbSet<Item> Items { get; set; }
       public DbSet<Poll> Polls {get; set; }
       public DbSet<Vote> Votes { get; set; }
    }
}
