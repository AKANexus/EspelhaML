using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class AlternateKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Questions_Id",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Pedidos_Id",
                table: "Pedidos",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PedidoPagamento_Id",
                table: "PedidoPagamento",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PedidoEnvio_Id",
                table: "PedidoEnvio",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PedidoDestinatário_Id",
                table: "PedidoDestinatário",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Itens_Id",
                table: "Itens",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ItemVariação_Id",
                table: "ItemVariação",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Questions_Id",
                table: "Questions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Pedidos_Id",
                table: "Pedidos");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PedidoPagamento_Id",
                table: "PedidoPagamento");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PedidoEnvio_Id",
                table: "PedidoEnvio");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PedidoDestinatário_Id",
                table: "PedidoDestinatário");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Itens_Id",
                table: "Itens");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ItemVariação_Id",
                table: "ItemVariação");
        }
    }
}
