using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MakerTracker.Migrations
{
    public partial class Needs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Need_Profiles_ProfileId",
                table: "Need");

            migrationBuilder.DropTable(
                name: "NeedDetail");

            migrationBuilder.DropColumn(
                name: "FulFillByDate",
                table: "Need");

            migrationBuilder.DropColumn(
                name: "RequestedOn",
                table: "Need");

            migrationBuilder.AddColumn<int>(
                name: "NeedId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "Profiles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Need",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "Need",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Need",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Need",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FulfilledDate",
                table: "Need",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Need",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Need",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SpecialInstructions",
                table: "Need",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfileHierarchy",
                columns: table => new
                {
                    ParentProfileId = table.Column<int>(nullable: false),
                    ChildProfileId = table.Column<int>(nullable: false),
                    ParentIsOwner = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileHierarchy", x => new { x.ParentProfileId, x.ChildProfileId });
                    table.ForeignKey(
                        name: "FK_ProfileHierarchy_Profiles_ChildProfileId",
                        column: x => x.ChildProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfileHierarchy_Profiles_ParentProfileId",
                        column: x => x.ParentProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_NeedId",
                table: "Transactions",
                column: "NeedId");

            migrationBuilder.CreateIndex(
                name: "IX_Need_ProductId",
                table: "Need",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileHierarchy_ChildProfileId",
                table: "ProfileHierarchy",
                column: "ChildProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Need_Products_ProductId",
                table: "Need",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Need_Profiles_ProfileId",
                table: "Need",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Need_NeedId",
                table: "Transactions",
                column: "NeedId",
                principalTable: "Need",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Need_Products_ProductId",
                table: "Need");

            migrationBuilder.DropForeignKey(
                name: "FK_Need_Profiles_ProfileId",
                table: "Need");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Need_NeedId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "ProfileHierarchy");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_NeedId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Need_ProductId",
                table: "Need");

            migrationBuilder.DropColumn(
                name: "NeedId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "Need");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Need");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Need");

            migrationBuilder.DropColumn(
                name: "FulfilledDate",
                table: "Need");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Need");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Need");

            migrationBuilder.DropColumn(
                name: "SpecialInstructions",
                table: "Need");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileId",
                table: "Need",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "FulFillByDate",
                table: "Need",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestedOn",
                table: "Need",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "NeedDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NeedId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NeedDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NeedDetail_Need_NeedId",
                        column: x => x.NeedId,
                        principalTable: "Need",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NeedDetail_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NeedDetail_NeedId",
                table: "NeedDetail",
                column: "NeedId");

            migrationBuilder.CreateIndex(
                name: "IX_NeedDetail_ProductId",
                table: "NeedDetail",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Need_Profiles_ProfileId",
                table: "Need",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
