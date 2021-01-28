using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class AddCountryAndOrigin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageHistory");

            migrationBuilder.AddColumn<long>(
                name: "OriginId",
                table: "TemporaryExposureKey",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "SendToGateway",
                table: "TemporaryExposureKey",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    IsGatewayEnabled = table.Column<bool>(nullable: false),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemporaryExposureKeyCountry",
                columns: table => new
                {
                    CountryId = table.Column<long>(nullable: false),
                    TemporaryExposureKeyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryExposureKeyCountry", x => new { x.TemporaryExposureKeyId, x.CountryId });
                    table.ForeignKey(
                        name: "FK_TemporaryExposureKeyCountry_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemporaryExposureKeyCountry_TemporaryExposureKey_TemporaryExposureKeyId",
                        column: x => x.TemporaryExposureKeyId,
                        principalTable: "TemporaryExposureKey",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            // ...
            migrationBuilder.InsertData(
               table: "Country",
               columns: new[] { "Id", "Code", "IsGatewayEnabled", "Name" },
               values: new object[,]
               {
                    { 1L, "AT", false, "Østrig" },
                    { 25L, "SI", false, "Slovenien" },
                    { 24L, "SK", false, "Slovakiet" },
                    { 23L, "RO", false, "Rumænien" },
                    { 22L, "PT", false, "Portugal" },
                    { 21L, "PL", false, "Polen" },
                    { 20L, "NL", false, "Holland" },
                    { 19L, "MT", false, "Malta" },
                    { 18L, "LU", false, "Luxembourg" },
                    { 17L, "LT", false, "Litauen" },
                    { 16L, "LV", false, "Letland" },
                    { 15L, "IT", false, "Italien" },
                    { 26L, "ES", false, "Spanien" },
                    { 14L, "IE", false, "Irland" },
                    { 12L, "GR", false, "Grækenland" },
                    { 11L, "DE", false, "Tyskland" },
                    { 10L, "FR", false, "Frankrig" },
                    { 9L, "FI", false, "Finland" },
                    { 8L, "EE", false, "Estland" },
                    { 7L, "DK", false, "Danmark" },
                    { 6L, "CZ", false, "Tjekkiet" },
                    { 5L, "CY", false, "Cypern" },
                    { 4L, "HR", false, "Kroatien" },
                    { 3L, "BG", false, "Bulgarien" },
                    { 2L, "BE", false, "Belgien" },
                    { 13L, "HU", false, "Ungarn" },
                    { 27L, "SE", false, "Sverige" }
               });

            // set OriginId to DK for all existing keys
            migrationBuilder.Sql("UPDATE [dbo].[TemporaryExposureKey] SET OriginId = 7");
           
            migrationBuilder.CreateIndex(
                name: "IX_TemporaryExposureKey_OriginId",
                table: "TemporaryExposureKey",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryExposureKeyCountry_CountryId",
                table: "TemporaryExposureKeyCountry",
                column: "CountryId");

            
            migrationBuilder.AddForeignKey(
               name: "FK_TemporaryExposureKey_Country_OriginId",
               table: "TemporaryExposureKey",
               column: "OriginId",
               principalTable: "Country",
               principalColumn: "Id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemporaryExposureKey_Country_OriginId",
                table: "TemporaryExposureKey");

            migrationBuilder.DropTable(
                name: "TemporaryExposureKeyCountry");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropIndex(
                name: "IX_TemporaryExposureKey_OriginId",
                table: "TemporaryExposureKey");

            migrationBuilder.DropColumn(
                name: "OriginId",
                table: "TemporaryExposureKey");

            migrationBuilder.DropColumn(
                name: "SendToGateway",
                table: "TemporaryExposureKey");

            migrationBuilder.CreateTable(
                name: "PackageHistory",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsValidPeriod = table.Column<bool>(type: "bit", nullable: false),
                    PackageTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageHistory", x => x.ID);
                });
        }
    }
}
