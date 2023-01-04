using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osekai.Octon.Database.EntityFramework.MySql.Migrations
{
    /// <inheritdoc />
    public partial class FixMedalRaritiesAndAddMedalSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedalRarity_Medals_MedalId",
                table: "MedalRarity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedalRarity",
                table: "MedalRarity");

            migrationBuilder.RenameTable(
                name: "MedalRarity",
                newName: "MedalRarities");

            migrationBuilder.RenameIndex(
                name: "IX_MedalRarity_MedalId",
                table: "MedalRarities",
                newName: "IX_MedalRarities_MedalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedalRarities",
                table: "MedalRarities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MedalSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MedalId = table.Column<int>(type: "int", nullable: false),
                    Locked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedalSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedalSettings_Medals_MedalId",
                        column: x => x.MedalId,
                        principalTable: "Medals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_MedalSettings_MedalId",
                table: "MedalSettings",
                column: "MedalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedalRarities_Medals_MedalId",
                table: "MedalRarities",
                column: "MedalId",
                principalTable: "Medals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedalRarities_Medals_MedalId",
                table: "MedalRarities");

            migrationBuilder.DropTable(
                name: "MedalSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedalRarities",
                table: "MedalRarities");

            migrationBuilder.RenameTable(
                name: "MedalRarities",
                newName: "MedalRarity");

            migrationBuilder.RenameIndex(
                name: "IX_MedalRarities_MedalId",
                table: "MedalRarity",
                newName: "IX_MedalRarity_MedalId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedalRarity",
                table: "MedalRarity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedalRarity_Medals_MedalId",
                table: "MedalRarity",
                column: "MedalId",
                principalTable: "Medals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
