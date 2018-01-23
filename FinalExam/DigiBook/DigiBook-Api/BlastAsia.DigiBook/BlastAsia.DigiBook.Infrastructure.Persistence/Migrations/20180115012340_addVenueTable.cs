using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Migrations
{
    public partial class addVenueTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Contact_GuestID",
                table: "Appointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venues",
                table: "Venues");

            migrationBuilder.RenameTable(
                name: "Venues",
                newName: "Venue");

            migrationBuilder.RenameColumn(
                name: "GuestID",
                table: "Appointment",
                newName: "GuestId");

            migrationBuilder.RenameColumn(
                name: "IsCanceleld",
                table: "Appointment",
                newName: "IsCancelled");

            migrationBuilder.RenameColumn(
                name: "AppointmnetDate",
                table: "Appointment",
                newName: "AppointmentDate");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_GuestID",
                table: "Appointment",
                newName: "IX_Appointment_GuestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venue",
                table: "Venue",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Contact_GuestId",
                table: "Appointment",
                column: "GuestId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Contact_GuestId",
                table: "Appointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venue",
                table: "Venue");

            migrationBuilder.RenameTable(
                name: "Venue",
                newName: "Venues");

            migrationBuilder.RenameColumn(
                name: "GuestId",
                table: "Appointment",
                newName: "GuestID");

            migrationBuilder.RenameColumn(
                name: "IsCancelled",
                table: "Appointment",
                newName: "IsCanceleld");

            migrationBuilder.RenameColumn(
                name: "AppointmentDate",
                table: "Appointment",
                newName: "AppointmnetDate");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_GuestId",
                table: "Appointment",
                newName: "IX_Appointment_GuestID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venues",
                table: "Venues",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Contact_GuestID",
                table: "Appointment",
                column: "GuestID",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
