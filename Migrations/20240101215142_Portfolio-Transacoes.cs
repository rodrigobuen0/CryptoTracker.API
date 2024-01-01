using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class PortfolioTransacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Portfolio",
                columns: table => new
                {
                    PortfolioID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AssetID = table.Column<int>(type: "int", nullable: false),
                    QuantidadeTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorMedio = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolio", x => x.PortfolioID);
                    table.ForeignKey(
                        name: "FK_Portfolio_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Portfolio_Ativos_AssetID",
                        column: x => x.AssetID,
                        principalTable: "Ativos",
                        principalColumn: "AssetID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Transacoes",
                columns: table => new
                {
                    TransactionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PortfolioID = table.Column<int>(type: "int", nullable: false),
                    AssetID = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoPorUnidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Taxa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataTransacao = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacoes", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transacoes_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transacoes_Ativos_AssetID",
                        column: x => x.AssetID,
                        principalTable: "Ativos",
                        principalColumn: "AssetID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transacoes_Portfolio_PortfolioID",
                        column: x => x.PortfolioID,
                        principalTable: "Portfolio",
                        principalColumn: "PortfolioID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_AssetID",
                table: "Portfolio",
                column: "AssetID");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolio_UserID",
                table: "Portfolio",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_AssetID",
                table: "Transacoes",
                column: "AssetID");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_PortfolioID",
                table: "Transacoes",
                column: "PortfolioID");

            migrationBuilder.CreateIndex(
                name: "IX_Transacoes_UserID",
                table: "Transacoes",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacoes");

            migrationBuilder.DropTable(
                name: "Portfolio");
        }
    }
}
