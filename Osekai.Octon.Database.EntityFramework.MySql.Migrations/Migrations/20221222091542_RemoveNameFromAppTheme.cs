using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osekai.Octon.Database.EntityFramework.MySql.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNameFromAppTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Name_idx",
                table: "AppTheme");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppTheme");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppTheme",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                collation: "ascii_general_ci")
                .Annotation("MySql:CharSet", "ascii");

            migrationBuilder.CreateIndex(
                name: "Name_idx",
                table: "AppTheme",
                column: "Name");
        }
    }
}
