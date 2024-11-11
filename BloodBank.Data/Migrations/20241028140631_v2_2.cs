using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBank.Data.Migrations
{
    /// <inheritdoc />
    public partial class v2_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Avarta",
                table: "Users",
                newName: "Avatar");

            migrationBuilder.AddColumn<Guid>(
                name: "HospitalId",
                table: "SessionDonors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionDonors_HospitalId",
                table: "SessionDonors",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionDonors_Users_HospitalId",
                table: "SessionDonors",
                column: "HospitalId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionDonors_Users_HospitalId",
                table: "SessionDonors");

            migrationBuilder.DropIndex(
                name: "IX_SessionDonors_HospitalId",
                table: "SessionDonors");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "SessionDonors");

            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "Users",
                newName: "Avarta");
        }
    }
}
