using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class PackSeparado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_PedidoEnvio_EnvioUuid",
                table: "Pedidos");

            migrationBuilder.DropTable(
                name: "PedidoEnvio");

            migrationBuilder.DropTable(
                name: "PedidoItem");

            migrationBuilder.DropTable(
                name: "PedidoPagamento");

            migrationBuilder.DropColumn(
                name: "PackId",
                table: "Pedidos");

            migrationBuilder.AddColumn<Guid>(
                name: "PackUuid",
                table: "Pedidos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Título = table.Column<string>(type: "text", nullable: false),
                    PreçoUnitário = table.Column<decimal>(type: "numeric", nullable: false),
                    QuantidadeVendida = table.Column<int>(type: "integer", nullable: false),
                    ItemUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DescritorVariação = table.Column<string>(type: "text", nullable: false),
                    ItemVariaçãoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    Sku = table.Column<string>(type: "text", nullable: false),
                    SeparaçãoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_OrderItem_ItemVariação_ItemVariaçãoUuid",
                        column: x => x.ItemVariaçãoUuid,
                        principalTable: "ItemVariação",
                        principalColumn: "Uuid");
                    table.ForeignKey(
                        name: "FK_OrderItem_Itens_ItemUuid",
                        column: x => x.ItemUuid,
                        principalTable: "Itens",
                        principalColumn: "Uuid");
                    table.ForeignKey(
                        name: "FK_OrderItem_Pedidos_OrderUuid",
                        column: x => x.OrderUuid,
                        principalTable: "Pedidos",
                        principalColumn: "Uuid");
                    table.ForeignKey(
                        name: "FK_OrderItem_SeparaçãoItem_SeparaçãoUuid",
                        column: x => x.SeparaçãoUuid,
                        principalTable: "SeparaçãoItem",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    TotalPago = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorTransação = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorRessarcido = table.Column<decimal>(type: "numeric", nullable: false),
                    Parcelas = table.Column<int>(type: "integer", nullable: false),
                    ValorFrete = table.Column<decimal>(type: "numeric", nullable: false),
                    OrderUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Uuid);
                    table.UniqueConstraint("AK_Payment_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Pedidos_OrderUuid",
                        column: x => x.OrderUuid,
                        principalTable: "Pedidos",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "Shipping",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SubStatus = table.Column<int>(type: "integer", nullable: true),
                    SubStatusDescrição = table.Column<string>(type: "text", nullable: true),
                    CriaçãoDoPedido = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ValorDeclarado = table.Column<decimal>(type: "numeric", nullable: true),
                    Largura = table.Column<decimal>(type: "numeric", nullable: true),
                    Altura = table.Column<decimal>(type: "numeric", nullable: true),
                    Comprimento = table.Column<decimal>(type: "numeric", nullable: true),
                    Peso = table.Column<decimal>(type: "numeric", nullable: true),
                    CódRastreamento = table.Column<string>(type: "text", nullable: true),
                    TipoEnvio = table.Column<int>(type: "integer", nullable: false),
                    DestinatárioUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipping", x => x.Uuid);
                    table.UniqueConstraint("AK_Shipping_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipping_PedidoDestinatário_DestinatárioUuid",
                        column: x => x.DestinatárioUuid,
                        principalTable: "PedidoDestinatário",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "Packs",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    ShippingUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packs", x => x.Uuid);
                    table.UniqueConstraint("AK_Packs_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packs_Shipping_ShippingUuid",
                        column: x => x.ShippingUuid,
                        principalTable: "Shipping",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_PackUuid",
                table: "Pedidos",
                column: "PackUuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ItemUuid",
                table: "OrderItem",
                column: "ItemUuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ItemVariaçãoUuid",
                table: "OrderItem",
                column: "ItemVariaçãoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderUuid",
                table: "OrderItem",
                column: "OrderUuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_SeparaçãoUuid",
                table: "OrderItem",
                column: "SeparaçãoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Packs_ShippingUuid",
                table: "Packs",
                column: "ShippingUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_OrderUuid",
                table: "Payment",
                column: "OrderUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_DestinatárioUuid",
                table: "Shipping",
                column: "DestinatárioUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Packs_PackUuid",
                table: "Pedidos",
                column: "PackUuid",
                principalTable: "Packs",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Shipping_EnvioUuid",
                table: "Pedidos",
                column: "EnvioUuid",
                principalTable: "Shipping",
                principalColumn: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Packs_PackUuid",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Shipping_EnvioUuid",
                table: "Pedidos");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Packs");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Shipping");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_PackUuid",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "PackUuid",
                table: "Pedidos");

            migrationBuilder.AddColumn<decimal>(
                name: "PackId",
                table: "Pedidos",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PedidoEnvio",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinatárioUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    Altura = table.Column<decimal>(type: "numeric", nullable: true),
                    Comprimento = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CriaçãoDoPedido = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CódRastreamento = table.Column<string>(type: "text", nullable: true),
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Largura = table.Column<decimal>(type: "numeric", nullable: true),
                    Peso = table.Column<decimal>(type: "numeric", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SubStatus = table.Column<int>(type: "integer", nullable: true),
                    SubStatusDescrição = table.Column<string>(type: "text", nullable: true),
                    TipoEnvio = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValorDeclarado = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoEnvio", x => x.Uuid);
                    table.UniqueConstraint("AK_PedidoEnvio_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoEnvio_PedidoDestinatário_DestinatárioUuid",
                        column: x => x.DestinatárioUuid,
                        principalTable: "PedidoDestinatário",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "PedidoItem",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    ItemVariaçãoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    SeparaçãoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DescritorVariação = table.Column<string>(type: "text", nullable: false),
                    PedidoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    PreçoUnitário = table.Column<decimal>(type: "numeric", nullable: false),
                    QuantidadeVendida = table.Column<int>(type: "integer", nullable: false),
                    Sku = table.Column<string>(type: "text", nullable: false),
                    Título = table.Column<string>(type: "text", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_PedidoItem_SeparaçãoItem_SeparaçãoUuid",
                        column: x => x.SeparaçãoUuid,
                        principalTable: "SeparaçãoItem",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateTable(
                name: "PedidoPagamento",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Parcelas = table.Column<int>(type: "integer", nullable: false),
                    PedidoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    TotalPago = table.Column<decimal>(type: "numeric", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValorFrete = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorRessarcido = table.Column<decimal>(type: "numeric", nullable: false),
                    ValorTransação = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoPagamento", x => x.Uuid);
                    table.UniqueConstraint("AK_PedidoPagamento_Id", x => x.Id);
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
                name: "IX_PedidoItem_SeparaçãoUuid",
                table: "PedidoItem",
                column: "SeparaçãoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoPagamento_PedidoUuid",
                table: "PedidoPagamento",
                column: "PedidoUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_PedidoEnvio_EnvioUuid",
                table: "Pedidos",
                column: "EnvioUuid",
                principalTable: "PedidoEnvio",
                principalColumn: "Uuid");
        }
    }
}
