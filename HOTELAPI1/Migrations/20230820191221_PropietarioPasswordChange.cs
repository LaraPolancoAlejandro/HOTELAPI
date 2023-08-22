using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOTELAPI1.Migrations
{
    /// <inheritdoc />
    public partial class PropietarioPasswordChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Propietarios",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Propietarios",
                newName: "PasswordHash");
        }
    }
}
