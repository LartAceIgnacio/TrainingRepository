using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Migrations
{
    public partial class RenameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appoinment_Contact_GuestId",
                table: "Appoinment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appoinment_Employee_HostId",
                table: "Appoinment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appoinment",
                table: "Appoinment");

            migrationBuilder.RenameTable(
                name: "Appoinment",
                newName: "Appointment");

            migrationBuilder.RenameIndex(
                name: "IX_Appoinment_HostId",
                table: "Appointment",
                newName: "IX_Appointment_HostId");

            migrationBuilder.RenameIndex(
                name: "IX_Appoinment_GuestId",
                table: "Appointment",
                newName: "IX_Appointment_GuestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "AppointmentId");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "Appoinment");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_HostId",
                table: "Appoinment",
                newName: "IX_Appoinment_HostId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_GuestId",
                table: "Appoinment",
                newName: "IX_Appoinment_GuestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appoinment",
                table: "Appoinment",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appoinment_Contact_GuestId",
                table: "Appoinment",
                column: "GuestId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appoinment_Employee_HostId",
                table: "Appoinment",
                column: "HostId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
