using Microsoft.EntityFrameworkCore.Migrations;

namespace MakerTracker.Migrations
{
    public partial class ProfileLatLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Profiles",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Profiles",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Profiles");
        }
    }
}
