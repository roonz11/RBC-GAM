using Microsoft.EntityFrameworkCore.Migrations;

namespace RBC_GAM.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialInstrument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrentPrice = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialInstrument", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialInstrumentUser",
                columns: table => new
                {
                    FinancialInstrumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialInstrumentUser", x => new { x.FinancialInstrumentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FinancialInstrumentUser_FinancialInstrument_FinancialInstrumentId",
                        column: x => x.FinancialInstrumentId,
                        principalTable: "FinancialInstrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FinancialInstrumentUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Threshold",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FinancialInstrumentId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threshold", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Threshold_FinancialInstrument_FinancialInstrumentId",
                        column: x => x.FinancialInstrumentId,
                        principalTable: "FinancialInstrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Threshold_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trigger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TriggerType = table.Column<int>(type: "INTEGER", nullable: false),
                    ThresholdId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    Diretion = table.Column<int>(type: "INTEGER", nullable: false),
                    Fluctuation = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trigger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trigger_Threshold_ThresholdId",
                        column: x => x.ThresholdId,
                        principalTable: "Threshold",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialInstrumentUser_UserId",
                table: "FinancialInstrumentUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Threshold_FinancialInstrumentId",
                table: "Threshold",
                column: "FinancialInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Threshold_UserId",
                table: "Threshold",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Trigger_ThresholdId",
                table: "Trigger",
                column: "ThresholdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialInstrumentUser");

            migrationBuilder.DropTable(
                name: "Trigger");

            migrationBuilder.DropTable(
                name: "Threshold");

            migrationBuilder.DropTable(
                name: "FinancialInstrument");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
