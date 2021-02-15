using Microsoft.EntityFrameworkCore.Migrations;

namespace RBC_GAM.Migrations
{
    public partial class FinancialInstrumentUser_DeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialInstrumentUser_FinancialInstrument_FinInstrumentId",
                table: "FinancialInstrumentUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialInstrumentUser_User_UserId",
                table: "FinancialInstrumentUser");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialInstrumentUser_FinancialInstrument_FinInstrumentId",
                table: "FinancialInstrumentUser",
                column: "FinInstrumentId",
                principalTable: "FinancialInstrument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialInstrumentUser_User_UserId",
                table: "FinancialInstrumentUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialInstrumentUser_FinancialInstrument_FinInstrumentId",
                table: "FinancialInstrumentUser");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialInstrumentUser_User_UserId",
                table: "FinancialInstrumentUser");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialInstrumentUser_FinancialInstrument_FinInstrumentId",
                table: "FinancialInstrumentUser",
                column: "FinInstrumentId",
                principalTable: "FinancialInstrument",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialInstrumentUser_User_UserId",
                table: "FinancialInstrumentUser",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
