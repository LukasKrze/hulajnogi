using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScooterRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRented",
                table: "Scooters");

            migrationBuilder.AddColumn<int>(
                name: "CurrentRentalId",
                table: "Scooters",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRentalId",
                table: "Scooters");

            migrationBuilder.AddColumn<bool>(
                name: "IsRented",
                table: "Scooters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
