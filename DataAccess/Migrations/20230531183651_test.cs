using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Types_States_StateId",
                table: "Types");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "States");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "States");

            migrationBuilder.AlterColumn<long>(
                name: "StateId",
                table: "Types",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Types_States_StateId",
                table: "Types",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Types_States_StateId",
                table: "Types");

            migrationBuilder.AlterColumn<long>(
                name: "StateId",
                table: "Types",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "States",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "States",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Types_States_StateId",
                table: "Types",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
