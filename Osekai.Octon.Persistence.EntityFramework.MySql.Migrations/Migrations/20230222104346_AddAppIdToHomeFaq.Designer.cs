﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Osekai.Octon.Persistence.EntityFramework.MySql;

#nullable disable

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Migrations
{
    [DbContext(typeof(MySqlOsekaiDbContext))]
    [Migration("20230222104346_AddAppIdToHomeFaq")]
    partial class AddAppIdToHomeFaq
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.App", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Experimental")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("SimpleName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .UseCollation("ascii_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("SimpleName"), "ascii");

                    b.Property<bool>("Visible")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Apps", (string)null);

                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb4_general_ci");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.AppTheme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AppId")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)")
                        .UseCollation("ascii_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Color"), "ascii");

                    b.Property<string>("DarkColor")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)")
                        .UseCollation("ascii_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("DarkColor"), "ascii");

                    b.Property<float>("DarkHslValueMultiplier")
                        .HasColumnType("float");

                    b.Property<bool>("HasCover")
                        .HasColumnType("tinyint(1)");

                    b.Property<float>("HslValueMultiplier")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("AppId")
                        .IsUnique();

                    b.HasIndex(new[] { "AppId" }, "fk_AppId_idx");

                    b.ToTable("AppThemes", (string)null);
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.BeatmapPack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BeatmapCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BeatmapPacks");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.BeatmapPackForMedal", b =>
                {
                    b.Property<int>("MedalId")
                        .HasColumnType("int");

                    b.Property<int>("BeatmapPackId")
                        .HasColumnType("int");

                    b.Property<int>("Gamemode")
                        .HasColumnType("int");

                    b.HasKey("MedalId", "BeatmapPackId", "Gamemode");

                    b.HasIndex("BeatmapPackId");

                    b.ToTable("BeatmapPacksForMedals");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.CacheEntry", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .UseCollation("ascii_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Name"), "ascii");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<DateTimeOffset>("ExpiresAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Name");

                    b.HasIndex("ExpiresAt");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("CacheEntries", (string)null);

                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb4_general_ci");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.HomeFaq", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AppId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LocalizationPrefix")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)")
                        .UseCollation("ascii_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("LocalizationPrefix"), "ascii");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("tinytext");

                    b.HasKey("Id");

                    b.ToTable("HomeFaqs", (string)null);

                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb4_general_ci");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.Locale", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("varchar");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("char");

                    b.Property<bool>("Experimental")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ExtraCss")
                        .HasColumnType("text");

                    b.Property<string>("ExtraHtml")
                        .HasColumnType("text");

                    b.Property<string>("Flag")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("char");

                    b.Property<bool>("Rtl")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Short")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("char");

                    b.Property<bool>("Wip")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Name");

                    b.ToTable("Locales");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.Medal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstAchievedBy")
                        .HasColumnType("tinytext");

                    b.Property<DateTimeOffset?>("FirstAchievedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Grouping")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar");

                    b.Property<string>("Instructions")
                        .HasColumnType("text");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar");

                    b.Property<int>("Ordering")
                        .HasColumnType("int");

                    b.Property<string>("Restriction")
                        .HasMaxLength(8)
                        .HasColumnType("varchar");

                    b.Property<string>("Video")
                        .HasColumnType("tinytext");

                    b.HasKey("Id");

                    b.ToTable("Medals", (string)null);

                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb4_general_ci");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.MedalRarity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<float>("Frequency")
                        .HasColumnType("float");

                    b.Property<int>("MedalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedalId")
                        .IsUnique();

                    b.ToTable("MedalRarities");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.MedalSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Locked")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MedalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MedalId")
                        .IsUnique();

                    b.ToTable("MedalSettings");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.MedalSolution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("MedalId")
                        .HasColumnType("int");

                    b.Property<int>("Mods")
                        .HasColumnType("int");

                    b.Property<string>("SubmittedBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .UseCollation("ascii_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("SubmittedBy"), "ascii");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MedalId")
                        .IsUnique();

                    b.ToTable("MedalSolutions");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.Session", b =>
                {
                    b.Property<string>("Token")
                        .HasMaxLength(32)
                        .HasColumnType("char")
                        .UseCollation("ascii_general_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Token"), "ascii");

                    b.Property<DateTimeOffset>("ExpiresAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Token");

                    b.ToTable("Sessions", (string)null);

                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb4_general_ci");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.UserGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Colour")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("ForceVisibleInComments")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Hidden")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Permissions")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.UserGroupsForUsers", b =>
                {
                    b.Property<int>("UserGroupId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserGroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGroupsForUsers");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.UserPermissionsOverride", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Permissions")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserPermissionsOverrides");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.AppTheme", b =>
                {
                    b.HasOne("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.App", "App")
                        .WithOne("AppTheme")
                        .HasForeignKey("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.AppTheme", "AppId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_AppId");

                    b.Navigation("App");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.BeatmapPackForMedal", b =>
                {
                    b.HasOne("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.BeatmapPack", "BeatmapPack")
                        .WithMany("MedalsForBeatmapPack")
                        .HasForeignKey("BeatmapPackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.Medal", "Medal")
                        .WithMany("BeatmapPacksForMedal")
                        .HasForeignKey("MedalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BeatmapPack");

                    b.Navigation("Medal");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.MedalRarity", b =>
                {
                    b.HasOne("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.Medal", "Medal")
                        .WithOne("Rarity")
                        .HasForeignKey("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.MedalRarity", "MedalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medal");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.MedalSettings", b =>
                {
                    b.HasOne("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.Medal", "Medal")
                        .WithOne("Settings")
                        .HasForeignKey("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.MedalSettings", "MedalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medal");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.MedalSolution", b =>
                {
                    b.HasOne("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.Medal", "Medal")
                        .WithOne("Solution")
                        .HasForeignKey("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.MedalSolution", "MedalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medal");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.UserGroupsForUsers", b =>
                {
                    b.HasOne("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.UserGroup", "UserGroup")
                        .WithMany("UserGroupForUsers")
                        .HasForeignKey("UserGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserGroup");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.App", b =>
                {
                    b.Navigation("AppTheme");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.BeatmapPack", b =>
                {
                    b.Navigation("MedalsForBeatmapPack");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.Medal", b =>
                {
                    b.Navigation("BeatmapPacksForMedal");

                    b.Navigation("Rarity");

                    b.Navigation("Settings");

                    b.Navigation("Solution");
                });

            modelBuilder.Entity("Osekai.Octon.Persistence.EntityFramework.MySql.Entities.UserGroup", b =>
                {
                    b.Navigation("UserGroupForUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
