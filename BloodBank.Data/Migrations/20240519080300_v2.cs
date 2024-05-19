using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBank.Data.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Hospitals",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Donors",
                newName: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Hospitals",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Donors",
                newName: "Email");
        }
    }
}
