using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class AddLithuaniaTranslationsToCountryEndpoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Translation",
                columns: new[] { "Id", "EntityId", "EntityName", "LanguageCountryId", "Value" },
                values: new object[,]
                {
                    { 241L, 1L, "Country", 31L, "Austrija" },
                    { 268L, 30L, "Country", 31L, "Islandija" },
                    { 267L, 27L, "Country", 31L, "Švedija" },
                    { 266L, 26L, "Country", 31L, "Ispanija" },
                    { 265L, 25L, "Country", 31L, "Slovėnija" },
                    { 264L, 24L, "Country", 31L, "Slovakija" },
                    { 263L, 23L, "Country", 31L, "Rumunija" },
                    { 262L, 22L, "Country", 31L, "Portugalija" },
                    { 261L, 21L, "Country", 31L, "Lenkija" },
                    { 260L, 20L, "Country", 31L, "Nyderlandai" },
                    { 259L, 19L, "Country", 31L, "Malta" },
                    { 258L, 18L, "Country", 31L, "Liuksemburgas" },
                    { 257L, 17L, "Country", 31L, "Lietuva" },
                    { 256L, 16L, "Country", 31L, "Latvija" },
                    { 255L, 15L, "Country", 31L, "Italija" },
                    { 254L, 14L, "Country", 31L, "Airija" },
                    { 253L, 13L, "Country", 31L, "Vengrija" },
                    { 252L, 12L, "Country", 31L, "Graikija" },
                    { 251L, 11L, "Country", 31L, "Vokietija" },
                    { 250L, 10L, "Country", 31L, "Prancūzija" },
                    { 249L, 9L, "Country", 31L, "Suomija" },
                    { 248L, 8L, "Country", 31L, "Estija" },
                    { 247L, 7L, "Country", 31L, "Danija" },
                    { 246L, 6L, "Country", 31L, "Čekijos Respublika" },
                    { 245L, 5L, "Country", 31L, "Kipras" },
                    { 244L, 4L, "Country", 31L, "Kroatija" },
                    { 243L, 3L, "Country", 31L, "Bulgarija" },
                    { 242L, 2L, "Country", 31L, "Belgija" },
                    { 269L, 31L, "Country", 31L, "Lichtenšteinas" },
                    { 270L, 32L, "Country", 31L, "Šveicarija " }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 241L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 242L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 243L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 244L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 245L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 246L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 247L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 248L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 249L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 250L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 251L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 252L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 253L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 254L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 255L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 256L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 257L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 258L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 259L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 260L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 261L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 262L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 263L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 264L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 265L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 266L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 267L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 268L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 269L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 270L);
        }
    }
}
