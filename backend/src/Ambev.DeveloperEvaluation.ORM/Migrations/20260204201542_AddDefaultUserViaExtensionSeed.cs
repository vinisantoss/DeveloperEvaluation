using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultUserViaExtensionSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Password", "Phone", "Role", "Status", "UpdatedAt", "Username" },
                values: new object[] { new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"), new DateTime(2026, 2, 4, 20, 15, 37, 79, DateTimeKind.Utc).AddTicks(1356), "dev.admin@ambev.com", "$2a$11$ffL71qrWZMo1NezZ4KiunO190phLmiNoGDUpYFMZi99GpcDZJGunG", "+5511987654321", "Admin", "Active", null, "dev_admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"));
        }
    }
}
