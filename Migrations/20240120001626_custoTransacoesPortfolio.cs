using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class custoTransacoesPortfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Custo",
                table: "Transacoes",
                type: "decimal(18,10)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CustoTotal",
                table: "Portfolio",
                type: "decimal(18,10)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Custo",
                table: "Transacoes");

            migrationBuilder.DropColumn(
                name: "CustoTotal",
                table: "Portfolio");
        }
    }
}
