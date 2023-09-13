using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class DestinatarioNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoEnvio_PedidoDestinatário_DestinatárioUuid",
                table: "PedidoEnvio");

            migrationBuilder.AlterColumn<Guid>(
                name: "DestinatárioUuid",
                table: "PedidoEnvio",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "Distrito",
                table: "PedidoDestinatário",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoEnvio_PedidoDestinatário_DestinatárioUuid",
                table: "PedidoEnvio",
                column: "DestinatárioUuid",
                principalTable: "PedidoDestinatário",
                principalColumn: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoEnvio_PedidoDestinatário_DestinatárioUuid",
                table: "PedidoEnvio");

            migrationBuilder.AlterColumn<Guid>(
                name: "DestinatárioUuid",
                table: "PedidoEnvio",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Distrito",
                table: "PedidoDestinatário",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoEnvio_PedidoDestinatário_DestinatárioUuid",
                table: "PedidoEnvio",
                column: "DestinatárioUuid",
                principalTable: "PedidoDestinatário",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
