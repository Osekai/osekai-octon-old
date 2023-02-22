using Microsoft.EntityFrameworkCore;
using Osekai.Octon.Domain;
using Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

namespace Osekai.Octon.Persistence.EntityFramework.MySql
{
    public class MySqlOsekaiDbContext : DbContext
    {
        internal DbSet<App> Apps { get; set; } = null!;
        internal DbSet<AppTheme> AppThemes { get; set; } = null!;
        internal DbSet<HomeFaq> Faqs { get; set; } = null!;
        internal DbSet<Session> Sessions { get; set; } = null!;
        internal DbSet<CacheEntry> CacheEntries { get; set; } = null!;
        internal DbSet<BeatmapPack> BeatmapPacks { get; set; } = null!;
        internal DbSet<Medal> Medals { get; set; } = null!;
        internal DbSet<BeatmapPackForMedal> BeatmapPacksForMedals { get; set; } = null!;
        internal DbSet<MedalSolution> MedalSolutions { get; set; } = null!;
        internal DbSet<MedalSettings> MedalSettings { get; set; } = null!;
        internal DbSet<MedalRarity> MedalRarities { get; set; } = null!;
        internal DbSet<UserGroup> UserGroups { get; set; } = null!;
        internal DbSet<UserGroupsForUsers> UserGroupsForUsers { get; set; } = null!;
        internal DbSet<UserPermissionsOverride> UserPermissionsOverrides { get; set; } = null!;
        internal DbSet<Locale> Locales { get; set; } = null!;

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
                    .HasMaxLength(11)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii");
                
                entity.Property(e => e.DarkColor)
                    .HasMaxLength(11)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii");

                entity.HasOne(d => d.App)
                    .WithOne(p => p.AppTheme)
                    .HasForeignKey<AppTheme>(d => d.AppId)
                    .OnDelete(DeleteBehavior.Cascade)
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

                entity.HasOne(e => e.App).WithMany(e => e.Faqs)
                    .HasForeignKey(e => e.AppId)
                    .OnDelete(DeleteBehavior.Cascade);

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

                entity.Property(e => e.Data).HasColumnType("longblob");
                entity.HasIndex(e => e.ExpiresAt);
            });

            modelBuilder.Entity<Medal>(entity =>
            {
                entity.ToTable("Medals");
                entity.UseCollation("utf8mb4_general_ci");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .HasColumnType("varchar")
                    .HasMaxLength(Specifications.MedalNameMaxLength);

                entity.Property(e => e.Description)
                    .HasColumnType("text");

                entity.Property(e => e.Instructions)
                    .HasColumnType("text");

                entity.Property(e => e.Link)
                    .HasColumnType("text");

                entity.Property(e => e.Video)
                    .HasColumnType("tinytext");

                entity.Property(e => e.FirstAchievedBy)
                    .HasColumnType("tinytext");

                entity.Property(e => e.Restriction)
                    .HasColumnType("varchar")
                    .HasMaxLength(Specifications.MedalRestrictionMaxLength);

                entity.Property(e => e.Grouping)
                    .HasColumnType("varchar")
                    .HasMaxLength(Specifications.MedalGroupingMaxLength);
            });

            modelBuilder.Entity<BeatmapPack>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<BeatmapPackForMedal>(entity =>
            {
                entity.HasKey(e => new { e.MedalId, e.BeatmapPackId, e.Gamemode });
                
                entity.HasOne(e => e.Medal).WithMany(e => e.BeatmapPacksForMedal).HasForeignKey(e => e.MedalId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.BeatmapPack).WithMany(e => e.MedalsForBeatmapPack)
                    .HasForeignKey(e => e.BeatmapPackId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MedalSolution>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Medal).WithOne(e => e.Solution)
                    .HasForeignKey<MedalSolution>(e => e.MedalId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.SubmittedBy)
                    .HasMaxLength(Specifications.MedalSolutionSubmittedByMaxLength)
                    .UseCollation("ascii_general_ci")
                    .HasCharSet("ascii")
                    .HasColumnType("varchar");

                entity.Property(e => e.Text)
                    .HasColumnType("text");
            });

            modelBuilder.Entity<MedalRarity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Medal).WithOne(e => e.Rarity).HasForeignKey<MedalRarity>(e => e.MedalId);
            });
            
            modelBuilder.Entity<MedalSettings>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Medal).WithOne(e => e.Settings).HasForeignKey<MedalSettings>(e => e.MedalId);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Description)
                    .HasColumnType("text");
                
                entity.Property(e => e.Name)
                    .HasMaxLength(Specifications.GroupNameMaxLength)
                    .HasColumnType("varchar");
                
                entity.Property(e => e.ShortName)
                    .HasMaxLength(Specifications.GroupShortNameMaxLength)
                    .HasColumnType("varchar");

                entity.Property(e => e.Permissions)
                    .HasColumnType("json");
                
                entity.Property(e => e.Colour)
                    .HasMaxLength(11);
            });

            modelBuilder.Entity<UserGroupsForUsers>(entity =>
            {
                entity.HasKey(e => new { UserGroupid = e.UserGroupId, e.UserId });
                
                entity.HasOne(e => e.UserGroup).WithMany(e => e.UserGroupForUsers)
                    .HasForeignKey(e => e.UserGroupId).OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<UserPermissionsOverride>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Permissions).HasColumnType("json");
                
                entity.HasIndex(e => e.UserId).IsUnique();
            });

            modelBuilder.Entity<Locale>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.Property(e => e.Code).HasColumnType("char").HasMaxLength(Specifications.LocaleCodeLength);
                entity.Property(e => e.Name).HasColumnType("varchar").HasMaxLength(Specifications.LocaleNameMaxLength);
                entity.Property(e => e.Short).HasColumnType("char").HasMaxLength(Specifications.LocaleShortLength);
                entity.Property(e => e.Flag).HasColumnType("char").HasMaxLength(Specifications.LocaleFlagMaxLength);

                entity.Property(e => e.ExtraCss).HasColumnType("text");
                entity.Property(e => e.ExtraHtml).HasColumnType("text");
            });
        }
    }
}
