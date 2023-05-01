using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockAnalysis.API.Migrations
{
    public partial class StockPercentChangeToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PercentageChange",
                table: "Stocks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageChange",
                table: "Stocks");
        }
    }
}
