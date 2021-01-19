using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class AdjustKeySource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE TemporaryExposureKey SET KeySource = 1000 WHERE KeySource=3");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
