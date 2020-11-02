using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class AddTranslations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Country");

            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(nullable: true),
                    EntityName = table.Column<string>(nullable: true),
                    EntityId = table.Column<long>(nullable: false),
                    EntityPropertyName = table.Column<string>(nullable: true),
                    LanguageCountryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translation_Country_LanguageCountryId",
                        column: x => x.LanguageCountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "Code", "IsGatewayEnabled" },
                values: new object[] { 28L, "EN", false });

            migrationBuilder.InsertData(
                table: "Translation",
                columns: new[] { "Id", "EntityId", "EntityName", "EntityPropertyName", "LanguageCountryId", "Value" },
                values: new object[,]
                {
                    { 25L, 25L, "Country", "NameInDanish", 7L, "Slovenien" },
                    { 24L, 24L, "Country", "NameInDanish", 7L, "Slovakiet" },
                    { 23L, 23L, "Country", "NameInDanish", 7L, "Rumænien" },
                    { 22L, 22L, "Country", "NameInDanish", 7L, "Portugal" },
                    { 21L, 21L, "Country", "NameInDanish", 7L, "Polen" },
                    { 20L, 20L, "Country", "NameInDanish", 7L, "Holland" },
                    { 19L, 19L, "Country", "NameInDanish", 7L, "Malta" },
                    { 18L, 18L, "Country", "NameInDanish", 7L, "Luxembourg" },
                    { 17L, 17L, "Country", "NameInDanish", 7L, "Litauen" },
                    { 16L, 16L, "Country", "NameInDanish", 7L, "Letland" },
                    { 15L, 15L, "Country", "NameInDanish", 7L, "Italien" },
                    { 14L, 14L, "Country", "NameInDanish", 7L, "Irland" },
                    { 13L, 13L, "Country", "NameInDanish", 7L, "Ungarn" },
                    { 12L, 12L, "Country", "NameInDanish", 7L, "Grækenland" },
                    { 11L, 11L, "Country", "NameInDanish", 7L, "Tyskland" },
                    { 10L, 10L, "Country", "NameInDanish", 7L, "Frankrig" },
                    { 9L, 9L, "Country", "NameInDanish", 7L, "Finland" },
                    { 8L, 8L, "Country", "NameInDanish", 7L, "Estland" },
                    { 7L, 7L, "Country", "NameInDanish", 7L, "Danmark" },
                    { 6L, 6L, "Country", "NameInDanish", 7L, "Tjekkiet" },
                    { 5L, 5L, "Country", "NameInDanish", 7L, "Cypern" },
                    { 4L, 4L, "Country", "NameInDanish", 7L, "Kroatien" },
                    { 3L, 3L, "Country", "NameInDanish", 7L, "Bulgarien" },
                    { 2L, 2L, "Country", "NameInDanish", 7L, "Belgien" },
                    { 1L, 1L, "Country", "NameInDanish", 7L, "Østrig" },
                    { 26L, 26L, "Country", "NameInDanish", 7L, "Spanien" },
                    { 27L, 27L, "Country", "NameInDanish", 7L, "Sverige" }
                });

            migrationBuilder.InsertData(
                table: "Translation",
                columns: new[] { "Id", "EntityId", "EntityName", "EntityPropertyName", "LanguageCountryId", "Value" },
                values: new object[,]
                {
                    { 28L, 1L, "Country", "NameInEnglish", 28L, "Austria" },
                    { 52L, 25L, "Country", "NameInEnglish", 28L, "Slovenia" },
                    { 51L, 24L, "Country", "NameInEnglish", 28L, "Slovakia" },
                    { 50L, 23L, "Country", "NameInEnglish", 28L, "Romania" },
                    { 49L, 22L, "Country", "NameInEnglish", 28L, "Portugal" },
                    { 48L, 21L, "Country", "NameInEnglish", 28L, "Poland" },
                    { 47L, 20L, "Country", "NameInEnglish", 28L, "Netherlands" },
                    { 46L, 19L, "Country", "NameInEnglish", 28L, "Malta" },
                    { 45L, 18L, "Country", "NameInEnglish", 28L, "Luxembourg" },
                    { 44L, 17L, "Country", "NameInEnglish", 28L, "Lithuania" },
                    { 43L, 16L, "Country", "NameInEnglish", 28L, "Latvia" },
                    { 42L, 15L, "Country", "NameInEnglish", 28L, "Italy" },
                    { 53L, 26L, "Country", "NameInEnglish", 28L, "Spain" },
                    { 41L, 14L, "Country", "NameInEnglish", 28L, "Ireland" },
                    { 39L, 12L, "Country", "NameInEnglish", 28L, "Greece" },
                    { 38L, 11L, "Country", "NameInEnglish", 28L, "Germany" },
                    { 37L, 10L, "Country", "NameInEnglish", 28L, "France" },
                    { 36L, 9L, "Country", "NameInEnglish", 28L, "Finland" },
                    { 35L, 8L, "Country", "NameInEnglish", 28L, "Estonia" },
                    { 34L, 7L, "Country", "NameInEnglish", 28L, "Denmark" },
                    { 33L, 6L, "Country", "NameInEnglish", 28L, "Czech Republic" },
                    { 32L, 5L, "Country", "NameInEnglish", 28L, "Cyprus" },
                    { 31L, 4L, "Country", "NameInEnglish", 28L, "Croatia" },
                    { 30L, 3L, "Country", "NameInEnglish", 28L, "Bulgaria" },
                    { 29L, 2L, "Country", "NameInEnglish", 28L, "Belgium" },
                    { 40L, 13L, "Country", "NameInEnglish", 28L, "Hungary" },
                    { 54L, 27L, "Country", "NameInEnglish", 28L, "Sweden" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Translation_LanguageCountryId",
                table: "Translation",
                column: "LanguageCountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Translation");

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Country",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "Østrig");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Belgien");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "Bulgarien");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Name",
                value: "Kroatien");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 5L,
                column: "Name",
                value: "Cypern");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 6L,
                column: "Name",
                value: "Tjekkiet");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 7L,
                column: "Name",
                value: "Danmark");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 8L,
                column: "Name",
                value: "Estland");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 9L,
                column: "Name",
                value: "Finland");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 10L,
                column: "Name",
                value: "Frankrig");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 11L,
                column: "Name",
                value: "Tyskland");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 12L,
                column: "Name",
                value: "Grækenland");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 13L,
                column: "Name",
                value: "Ungarn");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 14L,
                column: "Name",
                value: "Irland");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 15L,
                column: "Name",
                value: "Italien");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 16L,
                column: "Name",
                value: "Letland");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 17L,
                column: "Name",
                value: "Litauen");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 18L,
                column: "Name",
                value: "Luxembourg");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 19L,
                column: "Name",
                value: "Malta");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 20L,
                column: "Name",
                value: "Holland");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 21L,
                column: "Name",
                value: "Polen");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 22L,
                column: "Name",
                value: "Portugal");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 23L,
                column: "Name",
                value: "Rumænien");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 24L,
                column: "Name",
                value: "Slovakiet");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 25L,
                column: "Name",
                value: "Slovenien");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 26L,
                column: "Name",
                value: "Spanien");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "Id",
                keyValue: 27L,
                column: "Name",
                value: "Sverige");
        }
    }
}
