using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class MoreUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeparaçãoItem_PedidoItem_PedidoItemUuid",
                table: "SeparaçãoItem");

            migrationBuilder.DropForeignKey(
                name: "FK_SeparaçãoItem_Separações_SeparaçãoUuid",
                table: "SeparaçãoItem");

            migrationBuilder.DropIndex(
                name: "IX_SeparaçãoItem_PedidoItemUuid",
                table: "SeparaçãoItem");

            migrationBuilder.DropIndex(
                name: "IX_SeparaçãoItem_SeparaçãoUuid",
                table: "SeparaçãoItem");

            migrationBuilder.DropColumn(
                name: "PedidoItemUuid",
                table: "SeparaçãoItem");

            migrationBuilder.DropColumn(
                name: "SeparaçãoUuid",
                table: "SeparaçãoItem");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Usuários",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Usuários",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Usuários",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "Usuários",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SeparaçãoUuid",
                table: "PedidoItem",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_SeparaçãoUuid",
                table: "PedidoItem",
                column: "SeparaçãoUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PedidoItem_SeparaçãoItem_SeparaçãoUuid",
                table: "PedidoItem",
                column: "SeparaçãoUuid",
                principalTable: "SeparaçãoItem",
                principalColumn: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PedidoItem_SeparaçãoItem_SeparaçãoUuid",
                table: "PedidoItem");

            migrationBuilder.DropIndex(
                name: "IX_PedidoItem_SeparaçãoUuid",
                table: "PedidoItem");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Usuários");

            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "Usuários");

            migrationBuilder.DropColumn(
                name: "SeparaçãoUuid",
                table: "PedidoItem");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Usuários",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "Usuários",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PedidoItemUuid",
                table: "SeparaçãoItem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SeparaçãoUuid",
                table: "SeparaçãoItem",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeparaçãoItem_PedidoItemUuid",
                table: "SeparaçãoItem",
                column: "PedidoItemUuid");

            migrationBuilder.CreateIndex(
                name: "IX_SeparaçãoItem_SeparaçãoUuid",
                table: "SeparaçãoItem",
                column: "SeparaçãoUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_SeparaçãoItem_PedidoItem_PedidoItemUuid",
                table: "SeparaçãoItem",
                column: "PedidoItemUuid",
                principalTable: "PedidoItem",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeparaçãoItem_Separações_SeparaçãoUuid",
                table: "SeparaçãoItem",
                column: "SeparaçãoUuid",
                principalTable: "Separações",
                principalColumn: "Uuid");
        }
    }
}
