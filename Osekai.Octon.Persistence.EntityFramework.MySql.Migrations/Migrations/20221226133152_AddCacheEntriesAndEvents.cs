using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Migrations
{
    /// <inheritdoc />
    public partial class AddCacheEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CacheEntries",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "ascii_general_ci")
                        .Annotation("MySql:CharSet", "ascii"),
                    Data = table.Column<byte[]>(type: "blob", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CacheEntries", x => x.Name);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_CacheEntries_ExpiresAt",
                table: "CacheEntries",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_CacheEntries_Name",
                table: "CacheEntries",
                column: "Name",
                unique: true);

            migrationBuilder.Sql("CREATE EVENT RemoveExpiredSessions " + 
                                 "ON SCHEDULE AT CURRENT_TIMESTAMP + INTERVAL 1 MINUTE ON COMPLETION PRESERVE " +
                                 "DO " +
                                 "DELETE FROM Sessions WHERE NOW() >= ExpiresAt");
            
            migrationBuilder.Sql("CREATE EVENT RemoveExpiredCacheEntries " + 
                                 "ON SCHEDULE AT CURRENT_TIMESTAMP + INTERVAL 1 MINUTE ON COMPLETION PRESERVE " +
                                 "DO " +
                                 "DELETE FROM CacheEntries WHERE NOW() >= ExpiresAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CacheEntries");

            migrationBuilder.Sql("DROP EVENT RemoveExpiredSessions");
            migrationBuilder.Sql("DROP EVENT RemoveExpiredCacheEntries");
        }
    }
}
