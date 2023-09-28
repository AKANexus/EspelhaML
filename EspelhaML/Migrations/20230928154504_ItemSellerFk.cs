using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class ItemSellerFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Itens");

            migrationBuilder.AlterColumn<decimal>(
                name: "UserId",
                table: "MlUserAuthInfos",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<Guid>(
                name: "SellerUuid",
                table: "Itens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MlUserAuthInfos_UserId",
                table: "MlUserAuthInfos",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "PromolimitEntries",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantidadeAVenda = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Estoque = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromolimitEntries", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PromolimitEntries_Itens_ItemUuid",
                        column: x => x.ItemUuid,
                        principalTable: "Itens",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Itens_SellerUuid",
                table: "Itens",
                column: "SellerUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PromolimitEntries_ItemUuid",
                table: "PromolimitEntries",
                column: "ItemUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_MlUserAuthInfos_SellerUuid",
                table: "Itens",
                column: "SellerUuid",
                principalTable: "MlUserAuthInfos",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_MlUserAuthInfos_SellerUuid",
                table: "Itens");

            migrationBuilder.DropTable(
                name: "PromolimitEntries");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_MlUserAuthInfos_UserId",
                table: "MlUserAuthInfos");

            migrationBuilder.DropIndex(
                name: "IX_Itens_SellerUuid",
                table: "Itens");

            migrationBuilder.DropColumn(
                name: "SellerUuid",
                table: "Itens");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "MlUserAuthInfos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)");

            migrationBuilder.AddColumn<decimal>(
                name: "SellerId",
                table: "Itens",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
