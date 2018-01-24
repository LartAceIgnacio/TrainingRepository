using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Migrations
{
    public partial class AddedLLBBRR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BB",
                table: "Inventories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LL",
                table: "Inventories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RR",
                table: "Inventories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BB",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "LL",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "RR",
                table: "Inventories");
        }
    }
}
