using Microsoft.EntityFrameworkCore.Migrations;

namespace RBC_GAM.Migrations
{
    public partial class Removed_Thresholds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TriggerType",
                table: "Trigger",
                newName: "Action");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Action",
                table: "Trigger",
                newName: "TriggerType");
        }
    }
}
