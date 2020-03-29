using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MakerTracker.Migrations
{
    public partial class MoreTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Makers_Profiles_ProfileId",
                table: "Makers");

            migrationBuilder.DropTable(
                name: "MakerQueue");

            migrationBuilder.DropIndex(
                name: "IX_Makers_ProfileId",
                table: "Makers");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Makers");

            migrationBuilder.AddColumn<int>(
                name: "OwnerProfileId",
                table: "Makers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    OwnerProfileId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Profiles_OwnerProfileId",
                        column: x => x.OwnerProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MakerOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MakerId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    OrderedOn = table.Column<DateTime>(nullable: false),
                    ExpectedFinished = table.Column<DateTime>(nullable: false),
                    PromisedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MakerOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MakerOrder_Makers_MakerId",
                        column: x => x.MakerId,
                        principalTable: "Makers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MakerOrder_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MakerStock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MakerId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
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
                name: "CustomerOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: true),
                    RequestedOn = table.Column<DateTime>(nullable: false),
                    FulFillByDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOrder_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerStock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerStock_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerStock_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerOrderId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOrderDetail_CustomerOrder_CustomerOrderId",
                        column: x => x.CustomerOrderId,
                        principalTable: "CustomerOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrderDetail_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Makers_OwnerProfileId",
                table: "Makers",
                column: "OwnerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_OwnerProfileId",
                table: "Customer",
                column: "OwnerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrder_CustomerId",
                table: "CustomerOrder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetail_CustomerOrderId",
                table: "CustomerOrderDetail",
                column: "CustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderDetail_ProductId",
                table: "CustomerOrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStock_CustomerId",
                table: "CustomerStock",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerStock_ProductId",
                table: "CustomerStock",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerOrder_MakerId",
                table: "MakerOrder",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerOrder_ProductId",
                table: "MakerOrder",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerStock_MakerId",
                table: "MakerStock",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerStock_ProductId",
                table: "MakerStock",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Makers_Profiles_OwnerProfileId",
                table: "Makers",
                column: "OwnerProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Makers_Profiles_OwnerProfileId",
                table: "Makers");

            migrationBuilder.DropTable(
                name: "CustomerOrderDetail");

            migrationBuilder.DropTable(
                name: "CustomerStock");

            migrationBuilder.DropTable(
                name: "MakerOrder");

            migrationBuilder.DropTable(
                name: "MakerStock");

            migrationBuilder.DropTable(
                name: "CustomerOrder");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Makers_OwnerProfileId",
                table: "Makers");

            migrationBuilder.DropColumn(
                name: "OwnerProfileId",
                table: "Makers");

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "Makers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MakerQueue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompletedCount = table.Column<int>(type: "int", nullable: false),
                    MakerId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    PromisedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MakerQueue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MakerQueue_Makers_MakerId",
                        column: x => x.MakerId,
                        principalTable: "Makers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MakerQueue_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Makers_ProfileId",
                table: "Makers",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerQueue_MakerId",
                table: "MakerQueue",
                column: "MakerId");

            migrationBuilder.CreateIndex(
                name: "IX_MakerQueue_ProductId",
                table: "MakerQueue",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Makers_Profiles_ProfileId",
                table: "Makers",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
