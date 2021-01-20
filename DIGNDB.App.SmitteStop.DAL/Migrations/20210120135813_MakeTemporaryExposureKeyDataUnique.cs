using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class MakeTemporaryExposureKeyDataUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TemporaryExposureKey_KeyData",
                table: "TemporaryExposureKey",
                column: "KeyData",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TemporaryExposureKey_KeyData",
                table: "TemporaryExposureKey");
        }
    }
}
