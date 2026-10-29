using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandToComputer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Computers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Computers");
        }
    }
}
