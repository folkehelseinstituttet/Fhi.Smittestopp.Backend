using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class UpdateAppStatsDownloadsTotalName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSmittestopDownloads",
                table: "ApplicationStatistics");

            migrationBuilder.AddColumn<int>(
                name: "SmittestopDownloadsTotal",
                table: "ApplicationStatistics",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmittestopDownloadsTotal",
                table: "ApplicationStatistics");

            migrationBuilder.AddColumn<int>(
                name: "TotalSmittestopDownloads",
                table: "ApplicationStatistics",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
