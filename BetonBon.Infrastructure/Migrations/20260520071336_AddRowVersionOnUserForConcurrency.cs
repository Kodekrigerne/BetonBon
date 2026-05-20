using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BetonBon.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionOnUserForConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Users",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Users");
        }
    }
}
