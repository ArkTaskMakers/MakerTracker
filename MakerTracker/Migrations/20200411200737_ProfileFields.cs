using Microsoft.EntityFrameworkCore.Migrations;

namespace MakerTracker.Migrations
{
    public partial class ProfileFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsSelfQuarantined",
                table: "Profiles",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDropOffPoint",
                table: "Profiles",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Profiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Profiles");

            migrationBuilder.AlterColumn<bool>(
                name: "IsSelfQuarantined",
                table: "Profiles",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDropOffPoint",
                table: "Profiles",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
