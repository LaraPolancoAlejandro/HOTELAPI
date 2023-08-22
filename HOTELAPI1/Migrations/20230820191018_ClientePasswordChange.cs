using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOTELAPI1.Migrations
{
    /// <inheritdoc />
    public partial class ClientePasswordChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Clientes",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Clientes",
                newName: "PasswordHash");
        }
    }
}
