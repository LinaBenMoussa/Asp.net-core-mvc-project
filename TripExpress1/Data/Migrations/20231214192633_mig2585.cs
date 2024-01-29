using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripExpress.Data.Migrations
{
    public partial class mig2585 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "dateDebut",
                table: "ReservationRoom",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "dateFin",
                table: "ReservationRoom",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dateDebut",
                table: "ReservationRoom");

            migrationBuilder.DropColumn(
                name: "dateFin",
                table: "ReservationRoom");
        }
    }
}
