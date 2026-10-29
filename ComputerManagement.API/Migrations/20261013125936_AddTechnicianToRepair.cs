using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnicianToRepair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Technician",
                table: "Repairs",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Technician",
                table: "Repairs");
        }
    }
}
