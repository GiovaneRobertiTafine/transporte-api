using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransporteApi.Migrations
{
    /// <inheritdoc />
    public partial class Observacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observcoes",
                table: "Entregas");

            migrationBuilder.AddColumn<string>(
                name: "Observacoes",
                table: "Entregas",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observacoes",
                table: "Entregas");

            migrationBuilder.AddColumn<string>(
                name: "Observcoes",
                table: "Entregas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
