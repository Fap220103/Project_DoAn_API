using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class addShippingRelative : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShippingAddressId",
                table: "Order",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShippingAddressId",
                table: "Order",
                column: "ShippingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ShippingAddress_ShippingAddressId",
                table: "Order",
                column: "ShippingAddressId",
                principalTable: "ShippingAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_ShippingAddress_ShippingAddressId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShippingAddressId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                table: "Order");
        }
    }
}
