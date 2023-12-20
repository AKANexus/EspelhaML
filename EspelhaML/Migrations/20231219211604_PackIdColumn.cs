using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class PackIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PackId",
                table: "Pedidos",
                type: "numeric(20,0)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackId",
                table: "Pedidos");
        }
    }
}
