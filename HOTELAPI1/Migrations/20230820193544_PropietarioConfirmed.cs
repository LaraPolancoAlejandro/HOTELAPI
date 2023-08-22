using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOTELAPI1.Migrations
{
    /// <inheritdoc />
    public partial class PropietarioConfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Propietarios",
                newName: "Apellido");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "Propietarios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "Propietarios");

            migrationBuilder.RenameColumn(
                name: "Apellido",
                table: "Propietarios",
                newName: "Tipo");
        }
    }
}
