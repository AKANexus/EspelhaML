using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class ColumnRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Separações_Pedidos_PedidoId",
                table: "Separações");

            migrationBuilder.DropIndex(
                name: "IX_Separações_PedidoId",
                table: "Separações");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "Separações");

            migrationBuilder.AddColumn<decimal>(
                name: "Identificador",
                table: "Separações",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "SeparaçãoUuid",
                table: "Pedidos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_SeparaçãoUuid",
                table: "Pedidos",
                column: "SeparaçãoUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Separações_SeparaçãoUuid",
                table: "Pedidos",
                column: "SeparaçãoUuid",
                principalTable: "Separações",
                principalColumn: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Separações_SeparaçãoUuid",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_SeparaçãoUuid",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Identificador",
                table: "Separações");

            migrationBuilder.DropColumn(
                name: "SeparaçãoUuid",
                table: "Pedidos");

            migrationBuilder.AddColumn<Guid>(
                name: "PedidoId",
                table: "Separações",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Separações_PedidoId",
                table: "Separações",
                column: "PedidoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Separações_Pedidos_PedidoId",
                table: "Separações",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
