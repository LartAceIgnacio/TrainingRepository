using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace EFTraining.Data.Migrations
{
    public partial class ContactMiddleName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LasName",
                table: "Employee",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "LasName",
                table: "Contact",
                newName: "MiddleName");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Contact",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Contact");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Employee",
                newName: "LasName");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "Contact",
                newName: "LasName");
        }
    }
}
