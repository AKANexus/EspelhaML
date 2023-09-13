using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class ItensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Itens",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<string>(type: "text", nullable: false),
                    Título = table.Column<string>(type: "text", nullable: false),
                    SellerId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    PreçoVenda = table.Column<decimal>(type: "numeric", nullable: false),
                    QuantidadeÀVenda = table.Column<int>(type: "integer", nullable: false),
                    Permalink = table.Column<string>(type: "text", nullable: false),
                    PrimeiraFoto = table.Column<string>(type: "text", nullable: false),
                    ÉVariação = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Itens", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "ItemVariação",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    PreçoVenda = table.Column<decimal>(type: "numeric", nullable: false),
                    DescritorVariação = table.Column<string>(type: "text", nullable: false),
                    ItemUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemVariação", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_ItemVariação_Itens_ItemUuid",
                        column: x => x.ItemUuid,
                        principalTable: "Itens",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemVariação_ItemUuid",
                table: "ItemVariação",
                column: "ItemUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemVariação");

            migrationBuilder.DropTable(
                name: "Itens");
        }
    }
}
