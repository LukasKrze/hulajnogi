using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScooterRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedRentedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Scooters");

            migrationBuilder.AddColumn<bool>(
                name: "IsRented",
                table: "Scooters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRented",
                table: "Scooters");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Scooters",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
