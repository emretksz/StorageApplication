using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class UpdateStocks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Stores_StoreId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "Stores");

            migrationBuilder.AlterColumn<long>(
                name: "StoreId",
                table: "Stocks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Stores_StoreId",
                table: "Stocks",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stocks_Stores_StoreId",
                table: "Stocks");

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "Stores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "StoreId",
                table: "Stocks",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Stocks_Stores_StoreId",
                table: "Stocks",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }
    }
}
