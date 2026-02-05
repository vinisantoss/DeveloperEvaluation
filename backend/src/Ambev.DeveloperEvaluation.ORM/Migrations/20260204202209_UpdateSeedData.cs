using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BusinessPartners",
                columns: new[] { "Id", "Document", "Email", "ExternalId", "Name" },
                values: new object[,]
                {
                    { new Guid("93a85a69-d565-44d4-a3ca-3eff3034da19"), "12.345.678/0001-90", "contato@distribuidoraspaulo.com.br", "BP-001", "Distribuidora São Paulo" },
                    { new Guid("d1f057cd-b6e4-4913-a60e-76d8ba0e8bb5"), "98.765.432/0001-21", "vendas@distribuidorario.com.br", "BP-002", "Distribuidora Rio de Janeiro" }
                });

            migrationBuilder.InsertData(
                table: "OperationalUnits",
                columns: new[] { "Id", "ExternalId", "Location", "Name" },
                values: new object[,]
                {
                    { new Guid("661d37dc-ec33-47c7-8c50-d93cc70d97a1"), "OU-RJ-001", "Rua da Lapa, 50 - Rio de Janeiro, RJ", "Cervejaria Rio de Janeiro" },
                    { new Guid("78faca33-ddb5-4de6-81ad-458171371dfe"), "OU-SP-001", "Avenida Paulista, 1000 - São Paulo, SP", "Cervejaria São Paulo" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "ExternalId", "Name", "StandardPrice" },
                values: new object[,]
                {
                    { new Guid("61203461-022c-4ba3-8083-a83831c63e8e"), "Cerveja", "P-SKOL-002", "Skol Pilsen 350ml", 3.00m },
                    { new Guid("70c65fab-a341-4592-b48c-f51864a7eac5"), "Cerveja", "P-BRAHMA-001", "Brahma Chopp 600ml", 5.50m },
                    { new Guid("adec37cd-827a-4a6c-895c-01d1d1d435ef"), "Água", "P-AMA-005", "Água AMA 1,5L", 2.00m },
                    { new Guid("c5c46972-84fa-4fca-ae9d-cb81e626c8ae"), "Refrigerante", "P-GUARANA-003", "Guaraná Antarctica 2L", 7.00m },
                    { new Guid("e909ec36-4db0-4df0-8bb4-02f9a09b9680"), "Refrigerante", "P-H2OH-004", "H2OH! Limão 500ml", 4.50m }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 4, 20, 22, 8, 356, DateTimeKind.Utc).AddTicks(7487));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BusinessPartners",
                keyColumn: "Id",
                keyValue: new Guid("93a85a69-d565-44d4-a3ca-3eff3034da19"));

            migrationBuilder.DeleteData(
                table: "BusinessPartners",
                keyColumn: "Id",
                keyValue: new Guid("d1f057cd-b6e4-4913-a60e-76d8ba0e8bb5"));

            migrationBuilder.DeleteData(
                table: "OperationalUnits",
                keyColumn: "Id",
                keyValue: new Guid("661d37dc-ec33-47c7-8c50-d93cc70d97a1"));

            migrationBuilder.DeleteData(
                table: "OperationalUnits",
                keyColumn: "Id",
                keyValue: new Guid("78faca33-ddb5-4de6-81ad-458171371dfe"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("61203461-022c-4ba3-8083-a83831c63e8e"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("70c65fab-a341-4592-b48c-f51864a7eac5"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("adec37cd-827a-4a6c-895c-01d1d1d435ef"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("c5c46972-84fa-4fca-ae9d-cb81e626c8ae"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e909ec36-4db0-4df0-8bb4-02f9a09b9680"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a0eebc99-9c0b-4ef8-bb6d-6bb9bd380a11"),
                column: "CreatedAt",
                value: new DateTime(2026, 2, 4, 20, 15, 37, 79, DateTimeKind.Utc).AddTicks(1356));
        }
    }
}
