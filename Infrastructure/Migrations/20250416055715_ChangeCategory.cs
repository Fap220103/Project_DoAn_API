using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ChangeCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "SeoKeywords",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductCategory");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "ProductCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductCategory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ProductCategory",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "ProductCategory",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "ProductCategory",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoKeywords",
                table: "ProductCategory",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "ProductCategory",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductCategory",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "ProductCategory",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);
        }
    }
}
