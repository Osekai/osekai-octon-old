using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddAppIdToHomeFaq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppId",
                table: "HomeFaqs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppId",
                table: "HomeFaqs");
        }
    }
}
