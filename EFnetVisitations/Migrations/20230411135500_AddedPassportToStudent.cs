using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFnetVisitations.Migrations
{
    /// <inheritdoc />
    public partial class AddedPassportToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Passport_number",
                table: "Students",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Passport_series",
                table: "Students",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_FirstName_LastName",
                table: "Students",
                columns: new[] { "FirstName", "LastName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_FirstName_LastName",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Passport_number",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Passport_series",
                table: "Students");
        }
    }
}
