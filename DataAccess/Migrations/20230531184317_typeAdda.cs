using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class typeAdda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_States_StateId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Types_States_StateId",
                table: "Types");

            migrationBuilder.DropIndex(
                name: "IX_Types_StateId",
                table: "Types");

            migrationBuilder.DropIndex(
                name: "IX_Properties_StateId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Types");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Properties");

            migrationBuilder.AddColumn<long>(
                name: "PropertyId",
                table: "States",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TypeId",
                table: "States",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_States_PropertyId",
                table: "States",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_States_TypeId",
                table: "States",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_States_Properties_PropertyId",
                table: "States",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_States_Types_TypeId",
                table: "States",
                column: "TypeId",
                principalTable: "Types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_States_Properties_PropertyId",
                table: "States");

            migrationBuilder.DropForeignKey(
                name: "FK_States_Types_TypeId",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_States_PropertyId",
                table: "States");

            migrationBuilder.DropIndex(
                name: "IX_States_TypeId",
                table: "States");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "States");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "States");

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                table: "Types",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                table: "Properties",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Types_StateId",
                table: "Types",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_StateId",
                table: "Properties",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_States_StateId",
                table: "Properties",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Types_States_StateId",
                table: "Types",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id");
        }
    }
}
