using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ScooterRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class ExtendScooter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Scooters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Scooters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Scooters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ScooterCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScooterCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Scooters_CategoryId",
                table: "Scooters",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scooters_ScooterCategories_CategoryId",
                table: "Scooters",
                column: "CategoryId",
                principalTable: "ScooterCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scooters_ScooterCategories_CategoryId",
                table: "Scooters");

            migrationBuilder.DropTable(
                name: "ScooterCategories");

            migrationBuilder.DropIndex(
                name: "IX_Scooters_CategoryId",
                table: "Scooters");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Scooters");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Scooters");

            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Scooters");
        }
    }
}
