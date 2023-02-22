using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddAppRelationshipToHomeFaq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HomeFaqs_AppId",
                table: "HomeFaqs",
                column: "AppId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeFaqs_Apps_AppId",
                table: "HomeFaqs",
                column: "AppId",
                principalTable: "Apps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeFaqs_Apps_AppId",
                table: "HomeFaqs");

            migrationBuilder.DropIndex(
                name: "IX_HomeFaqs_AppId",
                table: "HomeFaqs");
        }
    }
}
