using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Osekai.Octon.Database.Models;

namespace Osekai.Octon.Database.EntityFramework
{
    public class MySqlOsekaiDbContext : DbContext
    {
        public DbSet<App> Apps { get; set; } = null!;
        public DbSet<AppTheme> AppThemes { get; set; } = null!;
        public DbSet<Faq> Faqs { get; set; } = null!;

        public MySqlOsekaiDbContext(DbContextOptions options) : base(options) {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<App>(entity =>
            {
                entity.ToTable("Apps");

                entity.UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Name).HasMaxLength(40);

                entity.Property(e => e.SimpleName)
                    .HasMaxLength(20)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii");
            });

            modelBuilder.Entity<AppTheme>(entity =>
            {
                entity.ToTable("AppTheme");

                entity.HasIndex(e => e.Name, "Name_idx");

                entity.HasIndex(e => e.AppId, "fk_AppId_idx");

                entity.Property(e => e.Color)
                    .HasMaxLength(11)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii");

                entity.HasOne(d => d.App)
                    .WithMany(p => p.AppThemes)
                    .HasForeignKey(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_AppId");
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("Faq");

                entity.UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.LocalizationPrefix)
                    .HasMaxLength(40)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii");

                entity.Property(e => e.Title).HasColumnType("tinytext");
            });
        }
    }
}
