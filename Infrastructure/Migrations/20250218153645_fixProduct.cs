using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class fixProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeature",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IsHome",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IsHot",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "SalePercent",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalePercent",
                table: "Product");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeature",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHome",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHot",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
