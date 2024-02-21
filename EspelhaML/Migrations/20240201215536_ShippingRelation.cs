using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class ShippingRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingId",
                table: "Embalagem");

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingUuid",
                table: "Embalagem",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "WebhookSubscribers",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    CallbackUrl = table.Column<string>(type: "text", nullable: false),
                    WebHookTopic = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookSubscribers", x => x.Uuid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Embalagem_ShippingUuid",
                table: "Embalagem",
                column: "ShippingUuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Embalagem_Shipping_ShippingUuid",
                table: "Embalagem",
                column: "ShippingUuid",
                principalTable: "Shipping",
                principalColumn: "Uuid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Embalagem_Shipping_ShippingUuid",
                table: "Embalagem");

            migrationBuilder.DropTable(
                name: "WebhookSubscribers");

            migrationBuilder.DropIndex(
                name: "IX_Embalagem_ShippingUuid",
                table: "Embalagem");

            migrationBuilder.DropColumn(
                name: "ShippingUuid",
                table: "Embalagem");

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingId",
                table: "Embalagem",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
