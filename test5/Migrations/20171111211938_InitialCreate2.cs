using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test5.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Inventory",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "detailedDescription",
                table: "Inventory",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "discountPrice",
                table: "Inventory",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "Inventory",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "price",
                table: "Inventory",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "detailedDescription",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "discountPrice",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "image",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "price",
                table: "Inventory");
        }
    }
}
