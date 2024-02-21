using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class TesteShippping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Embalagem_ShippingUuid",
                table: "Embalagem");

            migrationBuilder.CreateIndex(
                name: "IX_Embalagem_ShippingUuid",
                table: "Embalagem",
                column: "ShippingUuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Embalagem_ShippingUuid",
                table: "Embalagem");

            migrationBuilder.CreateIndex(
                name: "IX_Embalagem_ShippingUuid",
                table: "Embalagem",
                column: "ShippingUuid");
        }
    }
}
