using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class SeparacaoSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Pedidos_OrderUuid",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Pedidos_OrderUuid",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Packs_PackUuid",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Shipping_EnvioUuid",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Separações_Usuários_UsuárioUuid",
                table: "Separações");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Pedidos_Id",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos");

            migrationBuilder.RenameTable(
                name: "Pedidos",
                newName: "Orders");

            migrationBuilder.RenameColumn(
                name: "EnvioUuid",
                table: "Orders",
                newName: "ShippingUuid");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_PackUuid",
                table: "Orders",
                newName: "IX_Orders_PackUuid");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_EnvioUuid",
                table: "Orders",
                newName: "IX_Orders_ShippingUuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "UsuárioUuid",
                table: "Separações",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "Identificador",
                table: "Separações",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "SellerId",
                table: "Separações",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Orders_Id",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Orders_OrderUuid",
                table: "OrderItem",
                column: "OrderUuid",
                principalTable: "Orders",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Packs_PackUuid",
                table: "Orders",
                column: "PackUuid",
                principalTable: "Packs",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Shipping_ShippingUuid",
                table: "Orders",
                column: "ShippingUuid",
                principalTable: "Shipping",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Orders_OrderUuid",
                table: "Payment",
                column: "OrderUuid",
                principalTable: "Orders",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Separações_Usuários_UsuárioUuid",
                table: "Separações",
                column: "UsuárioUuid",
                principalTable: "Usuários",
                principalColumn: "Uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Orders_OrderUuid",
                table: "OrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Packs_PackUuid",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Shipping_ShippingUuid",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Orders_OrderUuid",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Separações_Usuários_UsuárioUuid",
                table: "Separações");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Orders_Id",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Separações");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Pedidos");

            migrationBuilder.RenameColumn(
                name: "ShippingUuid",
                table: "Pedidos",
                newName: "EnvioUuid");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ShippingUuid",
                table: "Pedidos",
                newName: "IX_Pedidos_EnvioUuid");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_PackUuid",
                table: "Pedidos",
                newName: "IX_Pedidos_PackUuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "UsuárioUuid",
                table: "Separações",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Identificador",
                table: "Separações",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Pedidos_Id",
                table: "Pedidos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos",
                column: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Pedidos_OrderUuid",
                table: "OrderItem",
                column: "OrderUuid",
                principalTable: "Pedidos",
                principalColumn: "Uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Pedidos_OrderUuid",
                table: "Payment",
                column: "OrderUuid",
                principalTable: "Pedidos",
                principalColumn: "Uuid");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Separações_Usuários_UsuárioUuid",
                table: "Separações",
                column: "UsuárioUuid",
                principalTable: "Usuários",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
