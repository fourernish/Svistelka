﻿using Microsoft.EntityFrameworkCore;

namespace Svistelka.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Micropost> Microposts { get; set; } = null!;
        public DbSet<Relation> Relations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Micropost>(entity =>
            {
                entity.Property(e => e.Content).HasColumnType("ntext");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Microposts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Microposts_ToUsers");
            });

            modelBuilder.Entity<Relation>(entity =>
            {
                entity.HasIndex(e => new { e.FollowerId, e.FollowedId }, "UniqPairFollowedFollower")
                    .IsUnique();

                entity.HasOne(d => d.Followed)
                    .WithMany(p => p.RelationFolloweds)
                    .HasForeignKey(d => d.FollowedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Followed");

                entity.HasOne(d => d.Follower)
                    .WithMany(p => p.RelationFollowers)
                    .HasForeignKey(d => d.FollowerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Follower");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(true);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(true);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(true);

                entity.Property(e => e.PasswordConfirmation)
                    .HasMaxLength(100)
                    .IsUnicode(true)
                    .HasColumnName("Password_Confirmation");
            });
        }
    }
}
