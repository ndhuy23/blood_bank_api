using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBank.Data.Migrations
{
    /// <inheritdoc />
    public partial class v1update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BloodType",
                table: "Donors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodType",
                table: "Donors");
        }
    }
}
