using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KernalTravelGuide.Migrations
{
    /// <inheritdoc />
    public partial class addavailaberoomcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Availability",
                table: "Restaurants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "AveragePrice",
                table: "Restaurants",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Restaurants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailableRooms",
                table: "Resorts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailableRooms",
                table: "Hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "AveragePrice",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "AvailableRooms",
                table: "Resorts");

            migrationBuilder.DropColumn(
                name: "AvailableRooms",
                table: "Hotels");
        }
    }
}
