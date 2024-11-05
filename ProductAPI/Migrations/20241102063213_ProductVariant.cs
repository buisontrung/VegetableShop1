using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProductVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInventorySupplier_Inventory_InventoryId",
                table: "ProductInventorySupplier");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInventorySupplier_Products_ProductId",
                table: "ProductInventorySupplier");

            migrationBuilder.DropIndex(
                name: "IX_ProductInventorySupplier_ProductId",
                table: "ProductInventorySupplier");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductInventorySupplier");

            migrationBuilder.RenameColumn(
                name: "WarehouseId",
                table: "ProductInventorySupplier",
                newName: "ProductVariantId");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "ProductInventorySupplier",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "InventoryId",
                table: "ProductInventorySupplier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductVariant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    VariantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariant_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventorySupplier_ProductVariantId",
                table: "ProductInventorySupplier",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_ProductId",
                table: "ProductVariant",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInventorySupplier_Inventory_InventoryId",
                table: "ProductInventorySupplier",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInventorySupplier_ProductVariant_ProductVariantId",
                table: "ProductInventorySupplier",
                column: "ProductVariantId",
                principalTable: "ProductVariant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInventorySupplier_Inventory_InventoryId",
                table: "ProductInventorySupplier");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInventorySupplier_ProductVariant_ProductVariantId",
                table: "ProductInventorySupplier");

            migrationBuilder.DropTable(
                name: "ProductVariant");

            migrationBuilder.DropIndex(
                name: "IX_ProductInventorySupplier_ProductVariantId",
                table: "ProductInventorySupplier");

            migrationBuilder.RenameColumn(
                name: "ProductVariantId",
                table: "ProductInventorySupplier",
                newName: "WarehouseId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "ProductInventorySupplier",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "InventoryId",
                table: "ProductInventorySupplier",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductInventorySupplier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventorySupplier_ProductId",
                table: "ProductInventorySupplier",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInventorySupplier_Inventory_InventoryId",
                table: "ProductInventorySupplier",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInventorySupplier_Products_ProductId",
                table: "ProductInventorySupplier",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
