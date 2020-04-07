using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MakerTracker.Migrations
{
    public partial class Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MakerOrder_Makers_MakerId",
                table: "MakerOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_MakerOrder_Products_ProductId",
                table: "MakerOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Profiles_FromId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Products_ProductId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Profiles_ToId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "MakerMaterial");

            migrationBuilder.DropTable(
                name: "MakerStock");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MakerOrder",
                table: "MakerOrder");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameTable(
                name: "MakerOrder",
                newName: "MakerOrders");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_ToId",
                table: "Transactions",
                newName: "IX_Transactions_ToId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_ProductId",
                table: "Transactions",
                newName: "IX_Transactions_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_FromId",
                table: "Transactions",
                newName: "IX_Transactions_FromId");

            migrationBuilder.RenameIndex(
                name: "IX_MakerOrder_ProductId",
                table: "MakerOrders",
                newName: "IX_MakerOrders_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_MakerOrder_MakerId",
                table: "MakerOrders",
                newName: "IX_MakerOrders_MakerId");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeprecated",
                table: "Products",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MakerOrders",
                table: "MakerOrders",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductRequirement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: true),
                    ChildId = table.Column<int>(nullable: true),
                    ChildQuantityRequired = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRequirement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRequirement_Products_ChildId",
                        column: x => x.ChildId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRequirement_Products_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequirement_ChildId",
                table: "ProductRequirement",
                column: "ChildId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRequirement_ParentId",
                table: "ProductRequirement",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MakerOrders_Makers_MakerId",
                table: "MakerOrders",
                column: "MakerId",
                principalTable: "Makers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MakerOrders_Products_ProductId",
                table: "MakerOrders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Profiles_FromId",
                table: "Transactions",
                column: "FromId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Products_ProductId",
                table: "Transactions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Profiles_ToId",
                table: "Transactions",
                column: "ToId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MakerOrders_Makers_MakerId",
                table: "MakerOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_MakerOrders_Products_ProductId",
                table: "MakerOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Profiles_FromId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Products_ProductId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Profiles_ToId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "ProductRequirement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MakerOrders",
                table: "MakerOrders");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsDeprecated",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameTable(
                name: "MakerOrders",
                newName: "MakerOrder");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ToId",
                table: "Transaction",
                newName: "IX_Transaction_ToId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ProductId",
                table: "Transaction",
                newName: "IX_Transaction_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_FromId",
                table: "Transaction",
                newName: "IX_Transaction_FromId");

            migrationBuilder.RenameIndex(
                name: "IX_MakerOrders_ProductId",
                table: "MakerOrder",
                newName: "IX_MakerOrder_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_MakerOrders_MakerId",
                table: "MakerOrder",
                newName: "IX_MakerOrder_MakerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MakerOrder",
                table: "MakerOrder",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MakerStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MakerId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MakerStock_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MakerMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MakerId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MakerMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MakerMaterial_Makers_MakerId",
                        column: x => x.MakerId,
                        principalTable: "Makers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MakerMaterial_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MakerMaterial_MakerId",
                table: "MakerMaterial",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerMaterial_MaterialId",
                table: "MakerMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerStock_MakerId",
                table: "MakerStock",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerStock_ProductId",
                table: "MakerStock",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MakerOrder_Makers_MakerId",
                table: "MakerOrder",
                column: "MakerId",
                principalTable: "Makers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MakerOrder_Products_ProductId",
                table: "MakerOrder",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Profiles_FromId",
                table: "Transaction",
                column: "FromId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Products_ProductId",
                table: "Transaction",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Profiles_ToId",
                table: "Transaction",
                column: "ToId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
