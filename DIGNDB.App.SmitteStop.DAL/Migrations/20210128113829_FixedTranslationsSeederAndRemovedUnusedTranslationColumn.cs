using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class FixedTranslationsSeederAndRemovedUnusedTranslationColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityPropertyName",
                table: "Translation");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 28L,
                column: "EntityId",
                value: 30L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 29L,
                column: "EntityId",
                value: 31L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 30L,
                column: "EntityId",
                value: 32L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 58L,
                column: "EntityId",
                value: 30L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 59L,
                column: "EntityId",
                value: 31L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 60L,
                column: "EntityId",
                value: 32L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 88L,
                column: "EntityId",
                value: 30L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 89L,
                column: "EntityId",
                value: 31L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 90L,
                column: "EntityId",
                value: 32L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntityPropertyName",
                table: "Translation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 28L,
                column: "EntityId",
                value: 28L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 29L,
                column: "EntityId",
                value: 29L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 30L,
                column: "EntityId",
                value: 30L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 58L,
                column: "EntityId",
                value: 28L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 59L,
                column: "EntityId",
                value: 29L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 60L,
                column: "EntityId",
                value: 30L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 88L,
                column: "EntityId",
                value: 28L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 89L,
                column: "EntityId",
                value: 29L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 90L,
                column: "EntityId",
                value: 30L);
        }
    }
}
