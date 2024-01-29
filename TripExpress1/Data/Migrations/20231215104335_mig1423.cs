using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripExpress.Data.Migrations
{
    public partial class mig1423 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Destinations_idHotel",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Hotels_idHotel",
                table: "Images",
                column: "idHotel",
                principalTable: "Hotels",
                principalColumn: "IdHotel",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Hotels_idHotel",
                table: "Images");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Destinations_idHotel",
                table: "Images",
                column: "idHotel",
                principalTable: "Destinations",
                principalColumn: "IdDestination",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
