using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComputerManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddStockToComputer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Computers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Computers");

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Computers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Computers");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Computers",
                type: "TEXT",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Computers",
                columns: new[] { "Id", "Name", "Processor", "Ram", "Status", "Type" },
                values: new object[,]
                {
                    { 1, "Desktop Computer 1", "Intel", "16 GB", 0, "Desktop" },
                    { 2, "Desktop Computer 2", "Ryzen", "8 GB", 1, "Desktop" },
                    { 3, "Laptop Computer 1", "Intel", "16 GB", 2, "Laptop" },
                    { 4, "Laptop Computer 2", "Ryzen", "32 GB", 0, "Laptop" },
                    { 5, "Desktop Computer 3", "Intel", "8 GB", 0, "Desktop" }
                });
        }
    }
}
