using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripExpress.Data.Migrations
{
    public partial class mig8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DateRooms",
                columns: table => new
                {
                    idDateRoom = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateDeb = table.Column<DateTime>(type: "datetime2", nullable: false),
                    datefin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    idRoom = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateRooms", x => x.idDateRoom);
                    table.ForeignKey(
                        name: "FK_DateRooms_Rooms_idRoom",
                        column: x => x.idRoom,
                        principalTable: "Rooms",
                        principalColumn: "IdRoom",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DateRooms_idRoom",
                table: "DateRooms",
                column: "idRoom");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DateRooms");
        }
    }
}
