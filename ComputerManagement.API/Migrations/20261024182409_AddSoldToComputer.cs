using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSoldToComputer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sold",
                table: "Computers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sold",
                table: "Computers");
        }
    }
}
