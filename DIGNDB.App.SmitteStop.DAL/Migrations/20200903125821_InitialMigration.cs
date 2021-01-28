using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackageHistory",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    PackageTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Size = table.Column<long>(nullable: false),
                    IsValidPeriod = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageHistory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TemporaryExposureKey",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false, defaultValueSql: "(newid())"),
                    KeyData = table.Column<byte[]>(maxLength: 255, nullable: false),
                    RollingStartNumber = table.Column<long>(nullable: false),
                    RollingPeriod = table.Column<long>(nullable: false),
                    TransmissionRiskLevel = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryExposureKey", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageHistory");

            migrationBuilder.DropTable(
                name: "TemporaryExposureKey");
        }
    }
}
