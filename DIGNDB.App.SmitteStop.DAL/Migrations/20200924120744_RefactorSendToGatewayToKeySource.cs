using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class RefactorSendToGatewayToKeySource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendToGateway",
                table: "TemporaryExposureKey");

            migrationBuilder.AddColumn<int>(
                name: "KeySource",
                table: "TemporaryExposureKey",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeySource",
                table: "TemporaryExposureKey");

            migrationBuilder.AddColumn<bool>(
                name: "SendToGateway",
                table: "TemporaryExposureKey",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
