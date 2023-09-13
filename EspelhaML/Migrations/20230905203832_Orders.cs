using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PedidoDestinatário",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Telefone = table.Column<string>(type: "text", nullable: false),
                    Logradouro = table.Column<string>(type: "text", nullable: false),
                    Número = table.Column<string>(type: "text", nullable: false),
                    CEP = table.Column<string>(type: "text", nullable: false),
                    Cidade = table.Column<string>(type: "text", nullable: false),
                    UF = table.Column<string>(type: "text", nullable: false),
                    Bairro = table.Column<string>(type: "text", nullable: false),
                    Distrito = table.Column<string>(type: "text", nullable: false),
                    ÉResidencial = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoDestinatário", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "PedidoEnvio",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SubStatus = table.Column<int>(type: "integer", nullable: true),
                    SubStatusDescrição = table.Column<string>(type: "text", nullable: false),
                    CriaçãoDoPedido = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ValorDeclarado = table.Column<decimal>(type: "numeric", nullable: true),
                    Largura = table.Column<decimal>(type: "numeric", nullable: false),
                    Altura = table.Column<decimal>(type: "numeric", nullable: false),
                    Comprimento = table.Column<decimal>(type: "numeric", nullable: false),
                    Peso = table.Column<decimal>(type: "numeric", nullable: false),
                    CódRastreamento = table.Column<string>(type: "text", nullable: false),
                    DestinatárioUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoEnvio", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PedidoEnvio_PedidoDestinatário_DestinatárioUuid",
                        column: x => x.DestinatárioUuid,
                        principalTable: "PedidoDestinatário",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Frete = table.Column<decimal>(type: "numeric", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EnvioUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Pedidos_PedidoEnvio_EnvioUuid",
                        column: x => x.EnvioUuid,
                        principalTable: "PedidoEnvio",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "PedidoItem",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Título = table.Column<string>(type: "text", nullable: false),
                    PreçoUnitário = table.Column<decimal>(type: "numeric", nullable: false),
                    QuantidadeVendida = table.Column<int>(type: "integer", nullable: false),
                    ItemUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DescritorVariação = table.Column<string>(type: "text", nullable: false),
                    ItemVariaçãoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    PedidoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItem", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PedidoItem_ItemVariação_ItemVariaçãoUuid",
                        column: x => x.ItemVariaçãoUuid,
                        principalTable: "ItemVariação",
                        principalColumn: "Uuid");
                    table.ForeignKey(
                        name: "FK_PedidoItem_Itens_ItemUuid",
                        column: x => x.ItemUuid,
                        principalTable: "Itens",
                        principalColumn: "Uuid");
                    table.ForeignKey(
                        name: "FK_PedidoItem_Pedidos_PedidoUuid",
                        column: x => x.PedidoUuid,
                        principalTable: "Pedidos",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "PedidoPagamento",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    TotalPago = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorTransação = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorRessarcido = table.Column<decimal>(type: "numeric", nullable: false),
                    Parcelas = table.Column<int>(type: "integer", nullable: false),
                    ValorFrete = table.Column<decimal>(type: "numeric", nullable: false),
                    PedidoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoPagamento", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_PedidoPagamento_Pedidos_PedidoUuid",
                        column: x => x.PedidoUuid,
                        principalTable: "Pedidos",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PedidoEnvio_DestinatárioUuid",
                table: "PedidoEnvio",
                column: "DestinatárioUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_ItemUuid",
                table: "PedidoItem",
                column: "ItemUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_ItemVariaçãoUuid",
                table: "PedidoItem",
                column: "ItemVariaçãoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItem_PedidoUuid",
                table: "PedidoItem",
                column: "PedidoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoPagamento_PedidoUuid",
                table: "PedidoPagamento",
                column: "PedidoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_EnvioUuid",
                table: "Pedidos",
                column: "EnvioUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoItem");

            migrationBuilder.DropTable(
                name: "PedidoPagamento");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "PedidoEnvio");

            migrationBuilder.DropTable(
                name: "PedidoDestinatário");
        }
    }
}
