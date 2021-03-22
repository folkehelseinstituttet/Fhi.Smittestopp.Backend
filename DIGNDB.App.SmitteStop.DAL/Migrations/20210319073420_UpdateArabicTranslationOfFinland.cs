using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class UpdateArabicTranslationOfFinland : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 99L,
                column: "Value",
                value: "فنلندا");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 99L,
                column: "Value",
                value: "فنلندة");
        }
    }
}
