using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Migrations
{
    public partial class AddedRelationshipineachtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Appointments_GuestId",
                table: "Appointments",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_HostId",
                table: "Appointments",
                column: "HostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Contacts_GuestId",
                table: "Appointments",
                column: "GuestId",
                principalTable: "Contacts",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Employees_HostId",
                table: "Appointments",
                column: "HostId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Contacts_GuestId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Employees_HostId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_GuestId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_HostId",
                table: "Appointments");
        }
    }
}
