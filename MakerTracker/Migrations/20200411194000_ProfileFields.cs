using Microsoft.EntityFrameworkCore.Migrations;

namespace MakerTracker.Migrations
{
    public partial class ProfileFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CompanyName",
                table: "Profiles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Profiles");
        }
    }
}
