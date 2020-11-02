using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class AddVisitedCountriesEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VisitedCountriesEnabled",
                table: "Country",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 2L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 3L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 4L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 5L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 6L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 7L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 8L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 9L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 10L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 11L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 12L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 13L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 14L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 15L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 16L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 17L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 18L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 19L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 20L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 21L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 22L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 23L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 24L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 25L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 26L,
                column: "VisitedCountriesEnabled",
                value: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 27L,
                column: "VisitedCountriesEnabled",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VisitedCountriesEnabled",
                table: "Country");
        }
    }
}