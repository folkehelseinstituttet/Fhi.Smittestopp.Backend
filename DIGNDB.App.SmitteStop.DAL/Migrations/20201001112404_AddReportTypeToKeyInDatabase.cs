using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class AddReportTypeToKeyInDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReportType",
                table: "TemporaryExposureKey",
                nullable: false,
                defaultValue: ReportType.CONFIRMED_TEST);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportType",
                table: "TemporaryExposureKey");
        }
    }
}
