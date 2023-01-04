using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osekai.Octon.Database.EntityFramework.MySql.Migrations
{
    /// <inheritdoc />
    public partial class CacheEntriesBlobToLongblob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "CacheEntries",
                type: "longblob",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "blob");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "CacheEntries",
                type: "blob",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "longblob");
        }
    }
}
