using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EncontraTuMascota.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicacionCerrada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Cerrada",
                table: "Publicaciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCierre",
                table: "Publicaciones",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Resolucion",
                table: "Publicaciones",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cerrada",
                table: "Publicaciones");

            migrationBuilder.DropColumn(
                name: "FechaCierre",
                table: "Publicaciones");

            migrationBuilder.DropColumn(
                name: "Resolucion",
                table: "Publicaciones");
        }
    }
}
