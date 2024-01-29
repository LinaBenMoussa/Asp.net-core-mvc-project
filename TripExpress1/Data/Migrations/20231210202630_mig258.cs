using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripExpress.Data.Migrations
{
    public partial class mig258 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Long",
                table: "Hotels",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "lat",
                table: "Hotels",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Long",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "lat",
                table: "Hotels");
        }
    }
}
