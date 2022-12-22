﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Osekai.Octon.Database.EntityFramework;

#nullable disable

namespace Osekai.Octon.Database.EntityFramework.MySql.Migrations
{
    [DbContext(typeof(MySqlOsekaiDbContext))]
    [Migration("20221222110043_OneToOneAppAppThemeRelationship")]
    partial class OneToOneAppAppThemeRelationship
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

            modelBuilder.Entity("Osekai.Octon.Database.EntityFramework.App", b =>
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

            modelBuilder.Entity("Osekai.Octon.Database.Models.AppTheme", b =>
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

                    b.ToTable("AppTheme", (string)null);
                });

            modelBuilder.Entity("Osekai.Octon.Database.Models.HomeFaq", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
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

                    b.ToTable("HomeFaq", (string)null);

                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb4_general_ci");
                });

            modelBuilder.Entity("Osekai.Octon.Database.Models.AppTheme", b =>
                {
                    b.HasOne("Osekai.Octon.Database.EntityFramework.App", "App")
                        .WithOne("AppThemes")
                        .HasForeignKey("Osekai.Octon.Database.Models.AppTheme", "AppId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired()
                        .HasConstraintName("fk_AppId");

                    b.Navigation("App");
                });

            modelBuilder.Entity("Osekai.Octon.Database.EntityFramework.App", b =>
                {
                    b.Navigation("AppThemes");
                });
#pragma warning restore 612, 618
        }
    }
}
