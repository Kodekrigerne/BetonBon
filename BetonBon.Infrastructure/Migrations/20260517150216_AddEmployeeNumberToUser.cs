using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetonBon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeNumberToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeNumber",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                table: "Users");
        }
    }
}
