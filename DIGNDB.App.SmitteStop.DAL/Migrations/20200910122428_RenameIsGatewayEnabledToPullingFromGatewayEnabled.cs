using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class RenameIsGatewayEnabledToPullingFromGatewayEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGatewayEnabled",
                table: "Country");

            migrationBuilder.AddColumn<bool>(
                name: "PullingFromGatewayEnabled",
                table: "Country",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PullingFromGatewayEnabled",
                table: "Country");

            migrationBuilder.AddColumn<bool>(
                name: "IsGatewayEnabled",
                table: "Country",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
