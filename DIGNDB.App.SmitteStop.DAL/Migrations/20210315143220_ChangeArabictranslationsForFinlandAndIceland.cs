using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class ChangeArabictranslationsForFinlandAndIceland : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 118L,
                column: "Value",
                value: "أيسلندا");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 219L,
                column: "Value",
                value: "فنلندا");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 118L,
                column: "Value",
                value: "ایسلندة");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 219L,
                column: "Value",
                value: "فن لینڈ");
        }
    }
}
