using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyExchange.Services.ExchangeRates.Migrations
{
    /// <inheritdoc />
    public partial class AddTradesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trades",
                columns: table => new
                {
                    TradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Result = table.Column<decimal>(type: "money", nullable: false),
                    AmountFrom = table.Column<decimal>(type: "money", nullable: false),
                    AmountTo = table.Column<decimal>(type: "money", nullable: false),
                    Rate = table.Column<decimal>(type: "money", nullable: false),
                    CurrencyFrom = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CurrencyTo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trades", x => x.TradeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trades");
        }
    }
}
