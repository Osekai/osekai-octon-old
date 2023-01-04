using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osekai.Octon.Database.EntityFramework.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddMedalSolution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_AppId",
                table: "AppThemes");

            migrationBuilder.CreateTable(
                name: "MedalSolutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MedalId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false, collation: "utf8mb4_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubmittedBy = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    Mods = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedalSolutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedalSolutions_Medals_MedalId",
                        column: x => x.MedalId,
                        principalTable: "Medals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_MedalSolutions_MedalId",
                table: "MedalSolutions",
                column: "MedalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_AppId",
                table: "AppThemes",
                column: "AppId",
                principalTable: "Apps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_AppId",
                table: "AppThemes");

            migrationBuilder.DropTable(
                name: "MedalSolutions");

            migrationBuilder.AddForeignKey(
                name: "fk_AppId",
                table: "AppThemes",
                column: "AppId",
                principalTable: "Apps",
                principalColumn: "Id");
        }
    }
}
