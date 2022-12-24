using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osekai.Octon.Database.EntityFramework.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HomeFaq",
                table: "HomeFaq");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppTheme",
                table: "AppTheme");

            migrationBuilder.RenameTable(
                name: "HomeFaq",
                newName: "HomeFaqs");

            migrationBuilder.RenameTable(
                name: "AppTheme",
                newName: "AppThemes");

            migrationBuilder.RenameIndex(
                name: "IX_AppTheme_AppId",
                table: "AppThemes",
                newName: "IX_AppThemes_AppId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomeFaqs",
                table: "HomeFaqs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppThemes",
                table: "AppThemes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Token = table.Column<string>(type: "char(32)", maxLength: 32, nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    Payload = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Token);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Token",
                table: "Sessions",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HomeFaqs",
                table: "HomeFaqs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppThemes",
                table: "AppThemes");

            migrationBuilder.RenameTable(
                name: "HomeFaqs",
                newName: "HomeFaq");

            migrationBuilder.RenameTable(
                name: "AppThemes",
                newName: "AppTheme");

            migrationBuilder.RenameIndex(
                name: "IX_AppThemes_AppId",
                table: "AppTheme",
                newName: "IX_AppTheme_AppId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HomeFaq",
                table: "HomeFaq",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppTheme",
                table: "AppTheme",
                column: "Id");
        }
    }
}
