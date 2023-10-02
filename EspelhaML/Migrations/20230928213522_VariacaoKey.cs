using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class VariacaoKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VariaçãoUuid",
                table: "PromolimitEntries",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromolimitEntries_VariaçãoUuid",
                table: "PromolimitEntries",
                column: "VariaçãoUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_PromolimitEntries_ItemVariação_VariaçãoUuid",
                table: "PromolimitEntries",
                column: "VariaçãoUuid",
                principalTable: "ItemVariação",
                principalColumn: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromolimitEntries_ItemVariação_VariaçãoUuid",
                table: "PromolimitEntries");

            migrationBuilder.DropIndex(
                name: "IX_PromolimitEntries_VariaçãoUuid",
                table: "PromolimitEntries");

            migrationBuilder.DropColumn(
                name: "VariaçãoUuid",
                table: "PromolimitEntries");
        }
    }
}
