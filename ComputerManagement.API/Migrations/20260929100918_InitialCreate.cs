using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ComputerManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Computers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Ram = table.Column<string>(type: "TEXT", nullable: false),
                    Processor = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Computers", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Computers");
        }
    }
}
