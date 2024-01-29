using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripExpress.Data.Migrations
{
    public partial class mig987 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdReservationRoom",
                table: "DateRooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DateRooms_IdReservationRoom",
                table: "DateRooms",
                column: "IdReservationRoom");

            migrationBuilder.AddForeignKey(
                name: "FK_DateRooms_ReservationRoom_IdReservationRoom",
                table: "DateRooms",
                column: "IdReservationRoom",
                principalTable: "ReservationRoom",
                principalColumn: "IdReservationRoom",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DateRooms_ReservationRoom_IdReservationRoom",
                table: "DateRooms");

            migrationBuilder.DropIndex(
                name: "IX_DateRooms_IdReservationRoom",
                table: "DateRooms");

            migrationBuilder.DropColumn(
                name: "IdReservationRoom",
                table: "DateRooms");
        }
    }
}
