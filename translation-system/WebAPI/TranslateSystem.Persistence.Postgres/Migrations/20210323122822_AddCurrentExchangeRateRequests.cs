using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TranslateSystem.Persistence.Postgre.Migrations
{
    public partial class AddCurrentExchangeRateRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentExchangeRateRequestId",
                table: "currency",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "current_exchange_rate_request",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_current_exchange_rate_request", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_currency_CurrentExchangeRateRequestId",
                table: "currency",
                column: "CurrentExchangeRateRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_currency_current_exchange_rate_request_CurrentExchangeRateR~",
                table: "currency",
                column: "CurrentExchangeRateRequestId",
                principalTable: "current_exchange_rate_request",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_currency_current_exchange_rate_request_CurrentExchangeRateR~",
                table: "currency");

            migrationBuilder.DropTable(
                name: "current_exchange_rate_request");

            migrationBuilder.DropIndex(
                name: "IX_currency_CurrentExchangeRateRequestId",
                table: "currency");

            migrationBuilder.DropColumn(
                name: "CurrentExchangeRateRequestId",
                table: "currency");
        }
    }
}
