using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Migrations
{
    /// <inheritdoc />
    public partial class FixEventStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP EVENT RemoveExpiredSessions");
            migrationBuilder.Sql("DROP EVENT RemoveExpiredCacheEntries");
            
            migrationBuilder.Sql("CREATE EVENT RemoveExpiredSessions " + 
                                 "ON SCHEDULE EVERY 1 MINUTE  " +
                                 "DO " +
                                 "DELETE FROM Sessions WHERE UTC_TIMESTAMP() >= ExpiresAt");
            
            migrationBuilder.Sql("CREATE EVENT RemoveExpiredCacheEntries " + 
                                 "ON SCHEDULE EVERY 1 MINUTE " +
                                 "DO " +
                                 "DELETE FROM CacheEntries WHERE UTC_TIMESTAMP() >= ExpiresAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP EVENT RemoveExpiredSessions");
            migrationBuilder.Sql("DROP EVENT RemoveExpiredCacheEntries");
        }
    }
}
