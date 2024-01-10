using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class NovoFluxoEmbalagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_SeparaçãoItem_SeparaçãoUuid",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Separações_SeparaçãoUuid",
                table: "Pedidos");

            migrationBuilder.DropTable(
                name: "SeparaçãoItem");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_SeparaçãoUuid",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_SeparaçãoUuid",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "Etiqueta",
                table: "Separações");

            migrationBuilder.DropColumn(
                name: "SeparaçãoUuid",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "SeparaçãoUuid",
                table: "OrderItem");

            migrationBuilder.CreateTable(
                name: "Embalagem",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Etiqueta = table.Column<string>(type: "text", nullable: true),
                    ShippingId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ReferenciaId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    TipoVendaMl = table.Column<int>(type: "integer", nullable: false),
                    StatusEmbalagem = table.Column<int>(type: "integer", nullable: false),
                    SeparaçãoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Embalagem", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Embalagem_Separações_SeparaçãoUuid",
                        column: x => x.SeparaçãoUuid,
                        principalTable: "Separações",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "EmbalagemItem",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    SKU = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    Descrição = table.Column<string>(type: "text", nullable: false),
                    QuantidadeAEscanear = table.Column<int>(type: "integer", nullable: false),
                    QuantidadeEscaneada = table.Column<int>(type: "integer", nullable: false),
                    EmbalagemUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmbalagemItem", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_EmbalagemItem_Embalagem_EmbalagemUuid",
                        column: x => x.EmbalagemUuid,
                        principalTable: "Embalagem",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Embalagem_ReferenciaId_TipoVendaMl",
                table: "Embalagem",
                columns: new[] { "ReferenciaId", "TipoVendaMl" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Embalagem_SeparaçãoUuid",
                table: "Embalagem",
                column: "SeparaçãoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_EmbalagemItem_EmbalagemUuid",
                table: "EmbalagemItem",
                column: "EmbalagemUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmbalagemItem");

            migrationBuilder.DropTable(
                name: "Embalagem");

            migrationBuilder.AddColumn<string>(
                name: "Etiqueta",
                table: "Separações",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SeparaçãoUuid",
                table: "Pedidos",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SeparaçãoUuid",
                table: "OrderItem",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SeparaçãoItem",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Separados = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeparaçãoItem", x => x.Uuid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_SeparaçãoUuid",
                table: "Pedidos",
                column: "SeparaçãoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_SeparaçãoUuid",
                table: "OrderItem",
                column: "SeparaçãoUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_SeparaçãoItem_SeparaçãoUuid",
                table: "OrderItem",
                column: "SeparaçãoUuid",
                principalTable: "SeparaçãoItem",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Separações_SeparaçãoUuid",
                table: "Pedidos",
                column: "SeparaçãoUuid",
                principalTable: "Separações",
                principalColumn: "Uuid");
        }
    }
}
