using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MakerTracker.Migrations
{
    public partial class StockToTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE dbo.MakerEquipment");
            migrationBuilder.DropForeignKey(
                name: "FK_MakerEquipment_Makers_MakerId",
                table: "MakerEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_MakerOrders_Makers_MakerId",
                table: "MakerOrders");

            migrationBuilder.DropTable(
                name: "MakerStock");

            migrationBuilder.DropTable(
                name: "Makers");

            migrationBuilder.DropIndex(
                name: "IX_MakerOrders_MakerId",
                table: "MakerOrders");

            migrationBuilder.DropIndex(
                name: "IX_MakerEquipment_MakerId",
                table: "MakerEquipment");

            migrationBuilder.DropColumn(
                name: "MakerId",
                table: "MakerOrders");

            migrationBuilder.DropColumn(
                name: "MakerId",
                table: "MakerEquipment");

            migrationBuilder.AddColumn<bool>(
                name: "HasCadSkills",
                table: "Profiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "MakerOrders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "MakerEquipment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MakerOrders_ProfileId",
                table: "MakerOrders",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerEquipment_ProfileId",
                table: "MakerEquipment",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_MakerEquipment_Profiles_ProfileId",
                table: "MakerEquipment",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MakerOrders_Profiles_ProfileId",
                table: "MakerOrders",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MakerEquipment_Profiles_ProfileId",
                table: "MakerEquipment");

            migrationBuilder.DropForeignKey(
                name: "FK_MakerOrders_Profiles_ProfileId",
                table: "MakerOrders");

            migrationBuilder.DropIndex(
                name: "IX_MakerOrders_ProfileId",
                table: "MakerOrders");

            migrationBuilder.DropIndex(
                name: "IX_MakerEquipment_ProfileId",
                table: "MakerEquipment");

            migrationBuilder.DropColumn(
                name: "HasCadSkills",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "MakerOrders");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "MakerEquipment");

            migrationBuilder.AddColumn<int>(
                name: "MakerId",
                table: "MakerOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MakerId",
                table: "MakerEquipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Makers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessToFaceMask = table.Column<bool>(type: "bit", nullable: false),
                    AccessToGloves = table.Column<bool>(type: "bit", nullable: false),
                    HasCadSkills = table.Column<bool>(type: "bit", nullable: false),
                    OwnerProfileId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Makers_Profiles_OwnerProfileId",
                        column: x => x.OwnerProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MakerStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MakerId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MakerStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MakerStock_Makers_MakerId",
                        column: x => x.MakerId,
                        principalTable: "Makers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MakerStock_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MakerOrders_MakerId",
                table: "MakerOrders",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerEquipment_MakerId",
                table: "MakerEquipment",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_Makers_OwnerProfileId",
                table: "Makers",
                column: "OwnerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerStock_MakerId",
                table: "MakerStock",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerStock_ProductId",
                table: "MakerStock",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MakerEquipment_Makers_MakerId",
                table: "MakerEquipment",
                column: "MakerId",
                principalTable: "Makers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MakerOrders_Makers_MakerId",
                table: "MakerOrders",
                column: "MakerId",
                principalTable: "Makers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
