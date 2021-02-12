using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class AddJwtTokenTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JwtToken",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ExpirationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JwtToken", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JwtToken");
        }
    }
}
