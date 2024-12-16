using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class addsaletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceSale",
                table: "ProductVariants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleId",
                table: "ProductVariants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_SaleId",
                table: "ProductVariants",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Sales_SaleId",
                table: "ProductVariants",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Sales_SaleId",
                table: "ProductVariants");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_SaleId",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "PriceSale",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "ProductVariants");
        }
    }
}
