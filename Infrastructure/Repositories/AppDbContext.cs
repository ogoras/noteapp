using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.LastLogin)
                .WithOne(l => l.User);
            modelBuilder.Entity<Note>()
                .HasOne(n => n.Owner)
                .WithMany(p => p.Notes);
            modelBuilder.Entity<Note>()
                .HasMany(n => n.ShareRecipients)
                .WithMany(p => p.NoteShares);
            modelBuilder.Entity<Photo>()
                .HasOne(p => p.Owner)
                .WithMany(p => p.Photos);
            modelBuilder.Entity<Photo>()
                .HasMany(p => p.ShareRecipients)
                .WithMany(p => p.PhotoShares);
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.Bio)
                .WithOne();
            modelBuilder.Entity<Profile>()
                .HasOne(p => p.ProfilePicture)
                .WithOne();
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Login> Logins { get; set; }
    }
}
