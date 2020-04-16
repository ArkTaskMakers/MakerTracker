using Microsoft.EntityFrameworkCore.Migrations;

namespace MakerTracker.Migrations
{
    public partial class InitProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequestor",
                table: "Profiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSupplier",
                table: "Profiles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequestor",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "IsSupplier",
                table: "Profiles");
        }
    }
}
