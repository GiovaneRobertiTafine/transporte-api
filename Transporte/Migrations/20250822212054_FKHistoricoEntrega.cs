using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransporteApi.Migrations
{
    /// <inheritdoc />
    public partial class FKHistoricoEntrega : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoEntregas_Entregas_EntregaId",
                table: "HistoricoEntregas");

            migrationBuilder.DropIndex(
                name: "IX_HistoricoEntregas_EntregaId",
                table: "HistoricoEntregas");

            migrationBuilder.DropColumn(
                name: "EntregaId",
                table: "HistoricoEntregas");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoEntregas_Entregas_Id",
                table: "HistoricoEntregas",
                column: "Id",
                principalTable: "Entregas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoricoEntregas_Entregas_Id",
                table: "HistoricoEntregas");

            migrationBuilder.AddColumn<string>(
                name: "EntregaId",
                table: "HistoricoEntregas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoEntregas_EntregaId",
                table: "HistoricoEntregas",
                column: "EntregaId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoricoEntregas_Entregas_EntregaId",
                table: "HistoricoEntregas",
                column: "EntregaId",
                principalTable: "Entregas",
                principalColumn: "Id");
        }
    }
}
