using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacteristicToComputer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Characteristic_ComputerType",
                table: "Computers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Characteristic_Processor",
                table: "Computers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Characteristic_RamGo",
                table: "Computers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Characteristic_StorageGo",
                table: "Computers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Characteristic_ComputerType",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "Characteristic_Processor",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "Characteristic_RamGo",
                table: "Computers");

            migrationBuilder.DropColumn(
                name: "Characteristic_StorageGo",
                table: "Computers");
        }
    }
}
