using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rent_A_Car.Migrations
{
    /// <inheritdoc />
    public partial class fixedpageview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarModel",
                table: "Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserEGN",
                table: "Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarModel",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "UserEGN",
                table: "Request");
        }
    }
}
