using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KernalTravelGuide.Migrations
{
    /// <inheritdoc />
    public partial class AddEntitySpecificFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResortId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TourPackageId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TouristSpotId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_HotelId",
                table: "Feedbacks",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ResortId",
                table: "Feedbacks",
                column: "ResortId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_RestaurantId",
                table: "Feedbacks",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_TouristSpotId",
                table: "Feedbacks",
                column: "TouristSpotId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_TourPackageId",
                table: "Feedbacks",
                column: "TourPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Hotels_HotelId",
                table: "Feedbacks",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Resorts_ResortId",
                table: "Feedbacks",
                column: "ResortId",
                principalTable: "Resorts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Restaurants_RestaurantId",
                table: "Feedbacks",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_TourPackages_TourPackageId",
                table: "Feedbacks",
                column: "TourPackageId",
                principalTable: "TourPackages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_TouristSpots_TouristSpotId",
                table: "Feedbacks",
                column: "TouristSpotId",
                principalTable: "TouristSpots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Hotels_HotelId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Resorts_ResortId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Restaurants_RestaurantId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_TourPackages_TourPackageId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_TouristSpots_TouristSpotId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_HotelId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ResortId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_RestaurantId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_TouristSpotId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_TourPackageId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "ResortId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "TourPackageId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "TouristSpotId",
                table: "Feedbacks");
        }
    }
}
