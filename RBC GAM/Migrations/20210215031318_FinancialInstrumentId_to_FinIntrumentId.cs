using Microsoft.EntityFrameworkCore.Migrations;

namespace RBC_GAM.Migrations
{
    public partial class FinancialInstrumentId_to_FinIntrumentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialInstrumentUser_FinancialInstrument_FinancialInstrumentId",
                table: "FinancialInstrumentUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Threshold_FinancialInstrument_FinancialInstrumentId",
                table: "Threshold");

            migrationBuilder.RenameColumn(
                name: "Diretion",
                table: "Trigger",
                newName: "Direction");

            migrationBuilder.RenameColumn(
                name: "FinancialInstrumentId",
                table: "FinancialInstrumentUser",
                newName: "FinInstrumentId");

            migrationBuilder.AlterColumn<int>(
                name: "FinancialInstrumentId",
                table: "Threshold",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "FinInstrumentId",
                table: "Threshold",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialInstrumentUser_FinancialInstrument_FinInstrumentId",
                table: "FinancialInstrumentUser",
                column: "FinInstrumentId",
                principalTable: "FinancialInstrument",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Threshold_FinancialInstrument_FinancialInstrumentId",
                table: "Threshold",
                column: "FinancialInstrumentId",
                principalTable: "FinancialInstrument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialInstrumentUser_FinancialInstrument_FinInstrumentId",
                table: "FinancialInstrumentUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Threshold_FinancialInstrument_FinancialInstrumentId",
                table: "Threshold");

            migrationBuilder.DropColumn(
                name: "FinInstrumentId",
                table: "Threshold");

            migrationBuilder.RenameColumn(
                name: "Direction",
                table: "Trigger",
                newName: "Diretion");

            migrationBuilder.RenameColumn(
                name: "FinInstrumentId",
                table: "FinancialInstrumentUser",
                newName: "FinancialInstrumentId");

            migrationBuilder.AlterColumn<int>(
                name: "FinancialInstrumentId",
                table: "Threshold",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialInstrumentUser_FinancialInstrument_FinancialInstrumentId",
                table: "FinancialInstrumentUser",
                column: "FinancialInstrumentId",
                principalTable: "FinancialInstrument",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Threshold_FinancialInstrument_FinancialInstrumentId",
                table: "Threshold",
                column: "FinancialInstrumentId",
                principalTable: "FinancialInstrument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
