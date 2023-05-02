using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockAnalysis.API.Migrations
{
    public partial class StockVerdictToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Verdict",
                table: "Stocks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verdict",
                table: "Stocks");
        }
    }
}
