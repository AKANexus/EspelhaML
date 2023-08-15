using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EspelhaML.Migrations
{
    /// <inheritdoc />
    public partial class InitialM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Caller = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "MlUserAuthInfos",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    AccessToken = table.Column<string>(type: "text", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MlUserAuthInfos", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    QuestionText = table.Column<string>(type: "text", nullable: false),
                    AskerId = table.Column<int>(type: "integer", nullable: false),
                    DateAsked = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateReplied = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    QuestionStatus = table.Column<string>(type: "text", nullable: false),
                    ItemMlb = table.Column<string>(type: "text", nullable: false),
                    AnswerStatus = table.Column<string>(type: "text", nullable: true),
                    AnswerText = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Uuid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "MlUserAuthInfos");

            migrationBuilder.DropTable(
                name: "Questions");
        }
    }
}
