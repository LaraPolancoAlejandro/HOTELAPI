using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace HOTELAPI1.Migrations
{
    public partial class ChangeGuidtoString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop Primary Key
            migrationBuilder.DropPrimaryKey(
                name: "PK_Propietarios",
                table: "Propietarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            // Alter Column
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Propietarios",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Clientes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // Add Primary Key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Propietarios",
                table: "Propietarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop Primary Key
            migrationBuilder.DropPrimaryKey(
                name: "PK_Propietarios",
                table: "Propietarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes");

            // Alter Column
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Propietarios",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Clientes",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Add Primary Key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Propietarios",
                table: "Propietarios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clientes",
                table: "Clientes",
                column: "Id");
        }
    }
}
