using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Migrations
{
    public partial class AddedRelationshipMigrationv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointmnet_Contact_GuestId",
                table: "Appointmnet");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointmnet_Employee_HostId",
                table: "Appointmnet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contact",
                table: "Contact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointmnet",
                table: "Appointmnet");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "Contact",
                newName: "Contacts");

            migrationBuilder.RenameTable(
                name: "Appointmnet",
                newName: "Appointments");

            migrationBuilder.RenameIndex(
                name: "IX_Appointmnet_HostId",
                table: "Appointments",
                newName: "IX_Appointments_HostId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointmnet_GuestId",
                table: "Appointments",
                newName: "IX_Appointments_GuestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts",
                column: "ContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "appointmentId");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contacts",
                table: "Contacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameTable(
                name: "Contacts",
                newName: "Contact");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointmnet");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_HostId",
                table: "Appointmnet",
                newName: "IX_Appointmnet_HostId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_GuestId",
                table: "Appointmnet",
                newName: "IX_Appointmnet_GuestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contact",
                table: "Contact",
                column: "ContactId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointmnet",
                table: "Appointmnet",
                column: "appointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointmnet_Contact_GuestId",
                table: "Appointmnet",
                column: "GuestId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointmnet_Employee_HostId",
                table: "Appointmnet",
                column: "HostId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
