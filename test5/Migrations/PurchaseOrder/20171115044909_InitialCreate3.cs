using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test5.Migrations.PurchaseOrder
{
    public partial class InitialCreate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    address1 = table.Column<string>(type: "TEXT", nullable: true),
                    address2 = table.Column<string>(type: "TEXT", nullable: true),
                    customerFirstName = table.Column<string>(type: "TEXT", nullable: true),
                    customerID = table.Column<int>(type: "INTEGER", nullable: false),
                    customerLastName = table.Column<string>(type: "TEXT", nullable: true),
                    datePurchased = table.Column<DateTime>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: true),
                    productID = table.Column<int>(type: "INTEGER", nullable: false),
                    productName = table.Column<string>(type: "TEXT", nullable: true),
                    shipDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    state = table.Column<string>(type: "TEXT", nullable: true),
                    stowLocation = table.Column<string>(type: "TEXT", nullable: true),
                    zip = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrder");
        }
    }
}
