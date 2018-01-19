using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Migrations
{
    public partial class ChangeforeignkeyContactIdtoGuestIdinAppoinment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Contact_ContactId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_ContactId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Appointment");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_GuestId",
                table: "Appointment",
                column: "GuestId");

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

            migrationBuilder.DropIndex(
                name: "IX_Appointment_GuestId",
                table: "Appointment");

            migrationBuilder.AddColumn<Guid>(
                name: "ContactId",
                table: "Appointment",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ContactId",
                table: "Appointment",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Contact_ContactId",
                table: "Appointment",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
