using Microsoft.EntityFrameworkCore.Migrations;

namespace MakerTracker.Migrations
{
    public partial class zipcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MakerOrder_Products_ProductId",
                table: "MakerOrder");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "MakerOrder",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MakerOrder_Products_ProductId",
                table: "MakerOrder",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MakerOrder_Products_ProductId",
                table: "MakerOrder");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "Profiles");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "MakerOrder",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_MakerOrder_Products_ProductId",
                table: "MakerOrder",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
