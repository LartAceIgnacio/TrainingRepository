using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Migrations
{
    public partial class Trytofixforeignkeyerror : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Employee_GuestId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Contact_HostId",
                table: "Appointment");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Contact_GuestId",
                table: "Appointment",
                column: "GuestId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Employee_HostId",
                table: "Appointment",
                column: "HostId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Contact_GuestId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Employee_HostId",
                table: "Appointment");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Employee_GuestId",
                table: "Appointment",
                column: "GuestId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Contact_HostId",
                table: "Appointment",
                column: "HostId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
