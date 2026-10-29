using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceToComputer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Computers",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Computers");
        }
    }
}
