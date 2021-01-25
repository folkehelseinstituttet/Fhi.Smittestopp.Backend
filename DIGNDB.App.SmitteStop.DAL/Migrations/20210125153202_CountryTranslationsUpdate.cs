using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class CountryTranslationsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 30L,
                column: "Code",
                value: "IS");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 31L,
                column: "Code",
                value: "LI");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 32L,
                column: "Code",
                value: "CH");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "Code", "PullingFromGatewayEnabled", "VisitedCountriesEnabled" },
                values: new object[] { "NB", false, false });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "Code", "PullingFromGatewayEnabled", "VisitedCountriesEnabled" },
                values: new object[] { 34L, "NN", false, false });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Austria" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Belgium" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Bulgaria" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Croatia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Cyprus" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Czech Republic" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Denmark" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Estonia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 9L,
                column: "LanguageCountryId",
                value: 28L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "France" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Germany" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Greece" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Hungary" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Ireland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Italy" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Latvia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Lithuania" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 18L,
                column: "LanguageCountryId",
                value: 28L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 19L,
                column: "LanguageCountryId",
                value: 28L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Netherlands" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Poland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 22L,
                column: "LanguageCountryId",
                value: 28L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Romania" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Slovakia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Slovenia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Spain" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 28L, "Sweden" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "EntityId", "Value" },
                values: new object[] { 28L, "Iceland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "EntityId", "Value" },
                values: new object[] { 29L, "Liechtenstein" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "EntityId", "Value" },
                values: new object[] { 30L, "Switzerland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 1L, 33L, "Østerrike" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 2L, 33L, "Belgia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 3L, 33L, "Bulgaria" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 4L, 33L, "Kroatia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 5L, 33L, "Kypros" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 6L, 33L, "Tsjekkia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 7L, 33L, "Danmark" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 8L, 33L, "Estland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 39L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 9L, 33L, "Finland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 40L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 10L, 33L, "Frankrike" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 41L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 11L, 33L, "Tyskland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 42L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 12L, 33L, "Hellas" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 43L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 13L, 33L, "Ungarn" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 44L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 14L, 33L, "Irland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 45L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 15L, 33L, "Italia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 46L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 16L, 33L, "Latvia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 47L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 17L, 33L, "Litauen" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 48L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 18L, 33L, "Luxembourg" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 49L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 19L, 33L, "Malta" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 50L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 20L, 33L, "Nederland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 51L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 21L, 33L, "Polen" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 52L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 22L, 33L, "Portugal" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 53L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 23L, 33L, "Romania" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 54L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 24L, 33L, "Slovakia" });

            migrationBuilder.InsertData(
                table: "Translation",
                columns: new[] { "Id", "EntityId", "EntityName", "EntityPropertyName", "LanguageCountryId", "Value" },
                values: new object[,]
                {
                    { 55L, 25L, "Country", null, 33L, "Slovenia" },
                    { 56L, 26L, "Country", null, 33L, "Spania" },
                    { 60L, 30L, "Country", null, 33L, "Sveits" },
                    { 58L, 28L, "Country", null, 33L, "Island" },
                    { 57L, 27L, "Country", null, 33L, "Sverige" },
                    { 59L, 29L, "Country", null, 33L, "Liechtenstein" }
                });

            migrationBuilder.InsertData(
                table: "Translation",
                columns: new[] { "Id", "EntityId", "EntityName", "EntityPropertyName", "LanguageCountryId", "Value" },
                values: new object[,]
                {
                    { 61L, 1L, "Country", null, 34L, "Austerrike" },
                    { 88L, 28L, "Country", null, 34L, "Island" },
                    { 87L, 27L, "Country", null, 34L, "Sverige" },
                    { 86L, 26L, "Country", null, 34L, "Spania" },
                    { 85L, 25L, "Country", null, 34L, "Slovenia" },
                    { 84L, 24L, "Country", null, 34L, "Slovakia" },
                    { 83L, 23L, "Country", null, 34L, "Romania" },
                    { 82L, 22L, "Country", null, 34L, "Portugal" },
                    { 81L, 21L, "Country", null, 34L, "Polen" },
                    { 80L, 20L, "Country", null, 34L, "Nederland" },
                    { 79L, 19L, "Country", null, 34L, "Malta" },
                    { 78L, 18L, "Country", null, 34L, "Luxembourg" },
                    { 77L, 17L, "Country", null, 34L, "Litauen" },
                    { 76L, 16L, "Country", null, 34L, "Latvia" },
                    { 75L, 15L, "Country", null, 34L, "Italia" },
                    { 74L, 14L, "Country", null, 34L, "Irland" },
                    { 73L, 13L, "Country", null, 34L, "Ungarn" },
                    { 72L, 12L, "Country", null, 34L, "Hellas" },
                    { 71L, 11L, "Country", null, 34L, "Tyskland" },
                    { 70L, 10L, "Country", null, 34L, "Frankrike" },
                    { 69L, 9L, "Country", null, 34L, "Finland" },
                    { 68L, 8L, "Country", null, 34L, "Estland" },
                    { 67L, 7L, "Country", null, 34L, "Danmark" },
                    { 66L, 6L, "Country", null, 34L, "Tsjekkia" },
                    { 65L, 5L, "Country", null, 34L, "Kypros" },
                    { 64L, 4L, "Country", null, 34L, "Kroatia" },
                    { 63L, 3L, "Country", null, 34L, "Bulgaria" },
                    { 62L, 2L, "Country", null, 34L, "Belgia" },
                    { 89L, 29L, "Country", null, 34L, "Liechtenstein" },
                    { 90L, 30L, "Country", null, 34L, "Sveits" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 55L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 56L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 57L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 58L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 59L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 60L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 61L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 62L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 63L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 64L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 65L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 66L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 67L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 68L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 69L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 70L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 71L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 72L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 73L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 74L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 75L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 76L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 77L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 78L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 79L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 80L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 81L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 82L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 83L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 84L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 85L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 86L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 87L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 88L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 89L);

            migrationBuilder.DeleteData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 90L);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 30L,
                column: "Code",
                value: "EL");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 31L,
                column: "Code",
                value: "IS");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 32L,
                column: "Code",
                value: "LI");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "Code", "PullingFromGatewayEnabled", "VisitedCountriesEnabled" },
                values: new object[] { "CH", true, true });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Østrig" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Belgien" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Bulgarien" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Kroatien" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Cypern" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Tjekkiet" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Danmark" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Estland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 9L,
                column: "LanguageCountryId",
                value: 29L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Frankrig" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Tyskland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Grækenland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 13L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Ungarn" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 14L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Irland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 15L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Italien" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 16L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Letland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 17L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Litauen" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 18L,
                column: "LanguageCountryId",
                value: 29L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 19L,
                column: "LanguageCountryId",
                value: 29L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 20L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Holland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 21L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Polen" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 22L,
                column: "LanguageCountryId",
                value: 29L);

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 23L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Rumænien" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 24L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Slovakiet" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 25L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Slovenien" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 26L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Spanien" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 27L,
                columns: new[] { "LanguageCountryId", "Value" },
                values: new object[] { 29L, "Sverige" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 28L,
                columns: new[] { "EntityId", "Value" },
                values: new object[] { 1L, "Austria" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 29L,
                columns: new[] { "EntityId", "Value" },
                values: new object[] { 2L, "Belgium" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 30L,
                columns: new[] { "EntityId", "Value" },
                values: new object[] { 3L, "Bulgaria" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 31L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 4L, 28L, "Croatia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 32L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 5L, 28L, "Cyprus" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 33L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 6L, 28L, "Czech Republic" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 34L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 7L, 28L, "Denmark" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 35L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 8L, 28L, "Estonia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 36L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 9L, 28L, "Finland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 37L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 10L, 28L, "France" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 38L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 11L, 28L, "Germany" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 39L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 12L, 28L, "Greece" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 40L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 13L, 28L, "Hungary" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 41L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 14L, 28L, "Ireland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 42L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 15L, 28L, "Italy" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 43L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 16L, 28L, "Latvia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 44L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 17L, 28L, "Lithuania" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 45L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 18L, 28L, "Luxembourg" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 46L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 19L, 28L, "Malta" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 47L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 20L, 28L, "Netherlands" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 48L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 21L, 28L, "Poland" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 49L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 22L, 28L, "Portugal" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 50L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 23L, 28L, "Romania" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 51L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 24L, 28L, "Slovakia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 52L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 25L, 28L, "Slovenia" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 53L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 26L, 28L, "Spain" });

            migrationBuilder.UpdateData(
                table: "Translation",
                keyColumn: "Id",
                keyValue: 54L,
                columns: new[] { "EntityId", "LanguageCountryId", "Value" },
                values: new object[] { 27L, 28L, "Sweden" });
        }
    }
}
