using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripExpress.Data.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerInfo",
                table: "ReservationRoom",
                newName: "UserName");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ReservationRoom",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "nbRoom",
                table: "ReservationRoom",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ReservationRoom");

            migrationBuilder.DropColumn(
                name: "nbRoom",
                table: "ReservationRoom");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "ReservationRoom",
                newName: "CustomerInfo");
        }
    }
}
