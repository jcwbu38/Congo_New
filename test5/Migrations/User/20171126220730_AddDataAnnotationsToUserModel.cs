using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace test5.Migrations.User
{
    public partial class AddDataAnnotationsToUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "zip",
                table: "User",
                newName: "Zip");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "User",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "last",
                table: "User",
                newName: "Last");

            migrationBuilder.RenameColumn(
                name: "first",
                table: "User",
                newName: "First");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "User",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "address2",
                table: "User",
                newName: "Address2");

            migrationBuilder.RenameColumn(
                name: "address1",
                table: "User",
                newName: "Address1");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "User",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Last",
                table: "User",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "First",
                table: "User",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Zip",
                table: "User",
                newName: "zip");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "User",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "Last",
                table: "User",
                newName: "last");

            migrationBuilder.RenameColumn(
                name: "First",
                table: "User",
                newName: "first");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "User",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Address2",
                table: "User",
                newName: "address2");

            migrationBuilder.RenameColumn(
                name: "Address1",
                table: "User",
                newName: "address1");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "User",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "last",
                table: "User",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "first",
                table: "User",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);
        }
    }
}
