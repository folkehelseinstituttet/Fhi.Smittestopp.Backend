using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class ChangesToCountryModelAndAddingMissingCountryCodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "PullingFromGatewayEnabled", "VisitedCountriesEnabled" },
                values: new object[] { true, true });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "Code", "PullingFromGatewayEnabled", "VisitedCountriesEnabled" },
                values: new object[,]
                {
                    { 33L, "CH", true, true },
                    { 30L, "EL", true, true },
                    { 31L, "IS", true, true },
                    { 32L, "LI", true, true }
                });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { null, 29L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 28L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 29L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 30L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 31L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 32L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 33L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 34L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 35L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 36L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 37L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 38L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 39L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 40L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 41L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 42L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 43L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 44L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 45L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 46L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 47L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 48L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 49L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 50L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 51L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 52L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 53L,
                column: "EntityPropertyName",
                value: null);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 54L,
                column: "EntityPropertyName",
                value: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "PullingFromGatewayEnabled", "VisitedCountriesEnabled" },
                values: new object[] { false, false });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 18L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 19L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 22L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "EntityPropertyName", "LanguageCountryId" },
                values: new object[] { "NameInDanish", 7L });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 28L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 29L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 30L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 31L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 32L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 33L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 34L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 35L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 36L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 37L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 38L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 39L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 40L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 41L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 42L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 43L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 44L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 45L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 46L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 47L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 48L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 49L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 50L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 51L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 52L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 53L,
                column: "EntityPropertyName",
                value: "NameInEnglish");

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 54L,
                column: "EntityPropertyName",
                value: "NameInEnglish");
        }
    }
}
