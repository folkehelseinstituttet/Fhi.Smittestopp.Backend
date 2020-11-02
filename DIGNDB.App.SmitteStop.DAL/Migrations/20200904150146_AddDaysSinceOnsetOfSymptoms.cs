using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class AddDaysSinceOnsetOfSymptoms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysSinceOnsetOfSymptoms",
                table: "TemporaryExposureKey",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysSinceOnsetOfSymptoms",
                table: "TemporaryExposureKey");
        }
    }
}
