using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MlSynch.Migrations
{
    /// <inheritdoc />
    public partial class AuthenticationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuários",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuários", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserInfoUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorIp = table.Column<string>(type: "text", nullable: false),
                    Revoked = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RevokerIp = table.Column<string>(type: "text", nullable: true),
                    ReplacedByToken = table.Column<string>(type: "text", nullable: true),
                    ReasonRevoked = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Usuários_UserInfoUuid",
                        column: x => x.UserInfoUuid,
                        principalTable: "Usuários",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Separações",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuárioUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Início = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Fim = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Etiqueta = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Separações", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_Separações_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Separações_Usuários_UsuárioUuid",
                        column: x => x.UsuárioUuid,
                        principalTable: "Usuários",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeparaçãoItem",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    PedidoItemUuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Separados = table.Column<int>(type: "integer", nullable: false),
                    SeparaçãoUuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeparaçãoItem", x => x.Uuid);
                    table.ForeignKey(
                        name: "FK_SeparaçãoItem_PedidoItem_PedidoItemUuid",
                        column: x => x.PedidoItemUuid,
                        principalTable: "PedidoItem",
                        principalColumn: "Uuid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeparaçãoItem_Separações_SeparaçãoUuid",
                        column: x => x.SeparaçãoUuid,
                        principalTable: "Separações",
                        principalColumn: "Uuid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserInfoUuid",
                table: "RefreshTokens",
                column: "UserInfoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_SeparaçãoItem_PedidoItemUuid",
                table: "SeparaçãoItem",
                column: "PedidoItemUuid");

            migrationBuilder.CreateIndex(
                name: "IX_SeparaçãoItem_SeparaçãoUuid",
                table: "SeparaçãoItem",
                column: "SeparaçãoUuid");

            migrationBuilder.CreateIndex(
                name: "IX_Separações_PedidoId",
                table: "Separações",
                column: "PedidoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Separações_UsuárioUuid",
                table: "Separações",
                column: "UsuárioUuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "SeparaçãoItem");

            migrationBuilder.DropTable(
                name: "Separações");

            migrationBuilder.DropTable(
                name: "Usuários");
        }
    }
}
