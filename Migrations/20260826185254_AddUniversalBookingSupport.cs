using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KernalTravelGuide.Migrations
{
    /// <inheritdoc />
    public partial class AddUniversalBookingSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_TourPackages_TourPackageId",
                table: "Bookings");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "TourPackageId",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BookingType",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfNights",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResortId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomsCount",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TouristSpotId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TravelInformationId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HotelId",
                table: "Bookings",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ResortId",
                table: "Bookings",
                column: "ResortId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RestaurantId",
                table: "Bookings",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TouristSpotId",
                table: "Bookings",
                column: "TouristSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TravelInformationId",
                table: "Bookings",
                column: "TravelInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Hotels_HotelId",
                table: "Bookings",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Resorts_ResortId",
                table: "Bookings",
                column: "ResortId",
                principalTable: "Resorts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Restaurants_RestaurantId",
                table: "Bookings",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_TourPackages_TourPackageId",
                table: "Bookings",
                column: "TourPackageId",
                principalTable: "TourPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_TouristSpots_TouristSpotId",
                table: "Bookings",
                column: "TouristSpotId",
                principalTable: "TouristSpots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_TravelInformations_TravelInformationId",
                table: "Bookings",
                column: "TravelInformationId",
                principalTable: "TravelInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Hotels_HotelId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Resorts_ResortId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Restaurants_RestaurantId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_TourPackages_TourPackageId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_TouristSpots_TouristSpotId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_TravelInformations_TravelInformationId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_HotelId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ResortId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_RestaurantId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TouristSpotId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_TravelInformationId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BookingType",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "NumberOfNights",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ResortId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "RoomsCount",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TouristSpotId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "TravelInformationId",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "TourPackageId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelId = table.Column<int>(type: "int", nullable: true),
                    ResortId = table.Column<int>(type: "int", nullable: true),
                    RestaurantId = table.Column<int>(type: "int", nullable: true),
                    TouristSpotId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_Resorts_ResortId",
                        column: x => x.ResortId,
                        principalTable: "Resorts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_TouristSpots_TouristSpotId",
                        column: x => x.TouristSpotId,
                        principalTable: "TouristSpots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_HotelId",
                table: "Reviews",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ResortId",
                table: "Reviews",
                column: "ResortId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RestaurantId",
                table: "Reviews",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TouristSpotId",
                table: "Reviews",
                column: "TouristSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_TourPackages_TourPackageId",
                table: "Bookings",
                column: "TourPackageId",
                principalTable: "TourPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
