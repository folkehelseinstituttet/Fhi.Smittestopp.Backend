using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class ChangesForDBModelForCovidStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SSIStatistics");

            migrationBuilder.CreateTable(
                name: "CovidStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfirmedCasesToday = table.Column<int>(nullable: false),
                    ConfirmedCasesTotal = table.Column<int>(nullable: false),
                    TestsConductedToday = table.Column<int>(nullable: false),
                    TestsConductedTotal = table.Column<int>(nullable: false),
                    PatientsAdmittedToday = table.Column<int>(nullable: false),
                    IcuAdmittedToday = table.Column<int>(nullable: false),
                    VaccinatedFirstDoseTotal = table.Column<double>(nullable: false),
                    VaccinatedFirstDoseToday = table.Column<double>(nullable: false),
                    VaccinatedSecondDoseTotal = table.Column<double>(nullable: false),
                    VaccinatedSecondDoseToday = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CovidStatistics", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CovidStatistics");

            migrationBuilder.CreateTable(
                name: "SSIStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfirmedCasesToday = table.Column<int>(type: "int", nullable: false),
                    ConfirmedCasesTotal = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientsAdmittedToday = table.Column<int>(type: "int", nullable: false),
                    TestsConductedToday = table.Column<int>(type: "int", nullable: false),
                    TestsConductedTotal = table.Column<int>(type: "int", nullable: false),
                    VaccinatedFirstDose = table.Column<double>(type: "float", nullable: false),
                    VaccinatedSecondDose = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SSIStatistics", x => x.Id);
                });
        }
    }
}
