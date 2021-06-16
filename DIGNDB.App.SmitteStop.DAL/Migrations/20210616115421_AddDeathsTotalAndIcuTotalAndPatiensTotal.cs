using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class AddDeathsTotalAndIcuTotalAndPatiensTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeathsCasesTotal",
                table: "CovidStatistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IcuAdmittedTotal",
                table: "CovidStatistics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PatientsAdmittedTotal",
                table: "CovidStatistics",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeathsCasesTotal",
                table: "CovidStatistics");

            migrationBuilder.DropColumn(
                name: "IcuAdmittedTotal",
                table: "CovidStatistics");

            migrationBuilder.DropColumn(
                name: "PatientsAdmittedTotal",
                table: "CovidStatistics");
        }
    }
}
