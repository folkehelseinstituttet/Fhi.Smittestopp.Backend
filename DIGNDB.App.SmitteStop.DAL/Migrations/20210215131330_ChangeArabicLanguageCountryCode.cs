using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class ChangeArabicLanguageCountryCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 35L,
                column: "Code",
                value: "AR");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 35L,
                column: "Code",
                value: "SA");
        }
    }
}
