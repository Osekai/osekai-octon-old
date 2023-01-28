using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddDarkColorToAppTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "HasCover",
                table: "AppTheme",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<string>(
                name: "DarkColor",
                table: "AppTheme",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "",
                collation: "ascii_general_ci")
                .Annotation("MySql:CharSet", "ascii");

            migrationBuilder.AddColumn<float>(
                name: "DarkHslValueMultiplier",
                table: "AppTheme",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<bool>(
                name: "Visible",
                table: "Apps",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<bool>(
                name: "Experimental",
                table: "Apps",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(sbyte),
                oldType: "tinyint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DarkColor",
                table: "AppTheme");

            migrationBuilder.DropColumn(
                name: "DarkHslValueMultiplier",
                table: "AppTheme");

            migrationBuilder.AlterColumn<sbyte>(
                name: "HasCover",
                table: "AppTheme",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<sbyte>(
                name: "Visible",
                table: "Apps",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<sbyte>(
                name: "Experimental",
                table: "Apps",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");
        }
    }
}
