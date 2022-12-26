using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Database.Models;

namespace Osekai.Octon.Database.EntityFramework.MySql
{
    public class MySqlOsekaiDbContext : DbContext
    {
        public DbSet<App> Apps { get; set; } = null!;
        public DbSet<AppTheme> AppThemes { get; set; } = null!;
        public DbSet<HomeFaq> Faqs { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        public DbSet<CacheEntry> CacheEntries { get; set; } = null!;

        public MySqlOsekaiDbContext(DbContextOptions options) : base(options) {}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<App>(entity =>
            {
                entity.ToTable("Apps");

                entity.UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Name).HasMaxLength(Specifications.AppNameMaxLength);

                entity.Property(e => e.SimpleName)
                    .HasMaxLength(Specifications.AppSimpleNameMaxLength)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii");
            });

            modelBuilder.Entity<AppTheme>(entity =>
            {
                entity.ToTable("AppThemes");

                entity.HasIndex(e => e.AppId, "fk_AppId_idx");

                entity.Property(e => e.Color)
                    .HasMaxLength(Specifications.AppColorMaxLength)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii");
                
                entity.Property(e => e.DarkColor)
                    .HasMaxLength(Specifications.AppColorMaxLength)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii");

                entity.HasOne(d => d.App)
                    .WithOne(p => p.AppTheme)
                    .HasForeignKey<AppTheme>(d => d.AppId)
                    .OnDelete(DeleteBehavior.ClientCascade)
                    .HasConstraintName("fk_AppId");
            });

            modelBuilder.Entity<HomeFaq>(entity =>
            {
                entity.ToTable("HomeFaqs");

                entity.UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Content).HasColumnType("text");

                entity.Property(e => e.LocalizationPrefix)
                    .HasMaxLength(Specifications.HomeFaqLocalizationPrefixMaxLength)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii");

                entity.Property(e => e.Title).HasColumnType("tinytext");
            });
            
            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Sessions");

                entity.UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Token)
                    .HasMaxLength(Specifications.SessionTokenLength)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii")
                    .HasColumnType("char");

                entity.HasKey(e => e.Token);
                entity.HasIndex(e => e.Token).IsUnique();
                
                entity.Property(e => e.Payload).HasColumnType("text");

                entity.Property(e => e.ExpiresAt).HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            });

            modelBuilder.Entity<CacheEntry>(entity =>
            {
                entity.ToTable("CacheEntries");

                entity.UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Name)
                    .HasMaxLength(Specifications.CacheEntryNameMaxLength)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii")
                    .HasColumnType("varchar");
                
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasKey(e => e.Name);

                entity.Property(e => e.Data).HasColumnType("blob");
                entity.HasIndex(e => e.ExpiresAt);
            });
        }
    }
}
