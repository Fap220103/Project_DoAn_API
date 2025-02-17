using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class fixRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Product_ProductId1",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_OrderId1",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product_ProductId1",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategory_ProductCategoryId1",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductColor_Product_ProductId1",
                table: "ProductColor");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSize_Product_ProductId1",
                table: "ProductSize");

            migrationBuilder.DropIndex(
                name: "IX_ProductSize_ProductId1",
                table: "ProductSize");

            migrationBuilder.DropIndex(
                name: "IX_ProductColor_ProductId1",
                table: "ProductColor");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductCategoryId1",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_OrderId1",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_ProductId1",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_ProductId1",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "ProductSize");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "ProductColor");

            migrationBuilder.DropColumn(
                name: "ProductCategoryId1",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Inventory");

            migrationBuilder.AlterColumn<string>(
                name: "ProductCategoryId",
                table: "Product",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductId1",
                table: "ProductSize",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId1",
                table: "ProductColor",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCategoryId",
                table: "Product",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddColumn<string>(
                name: "ProductCategoryId1",
                table: "Product",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderId1",
                table: "OrderDetail",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId1",
                table: "OrderDetail",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId1",
                table: "Inventory",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSize_ProductId1",
                table: "ProductSize",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColor_ProductId1",
                table: "ProductColor",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductCategoryId1",
                table: "Product",
                column: "ProductCategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId1",
                table: "OrderDetail",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductId1",
                table: "OrderDetail",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ProductId1",
                table: "Inventory",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Product_ProductId1",
                table: "Inventory",
                column: "ProductId1",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_OrderId1",
                table: "OrderDetail",
                column: "OrderId1",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product_ProductId1",
                table: "OrderDetail",
                column: "ProductId1",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategory_ProductCategoryId1",
                table: "Product",
                column: "ProductCategoryId1",
                principalTable: "ProductCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductColor_Product_ProductId1",
                table: "ProductColor",
                column: "ProductId1",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSize_Product_ProductId1",
                table: "ProductSize",
                column: "ProductId1",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}
