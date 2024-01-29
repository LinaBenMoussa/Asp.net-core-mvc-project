using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripExpress.Data.Migrations
{
    public partial class mig44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_HotelIdHotel",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_HotelIdHotel",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "HotelIdHotel",
                table: "Images");

            migrationBuilder.CreateIndex(
                name: "IX_Images_idHotel",
                table: "Images",
                column: "idHotel");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Destinations_idHotel",
                table: "Images",
                column: "idHotel",
                principalTable: "Destinations",
                principalColumn: "IdDestination",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Destinations_idHotel",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_idHotel",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "HotelIdHotel",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Images_HotelIdHotel",
                table: "Images",
                column: "HotelIdHotel");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Hotels_HotelIdHotel",
                table: "Images",
                column: "HotelIdHotel",
                principalTable: "Hotels",
                principalColumn: "IdHotel",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
