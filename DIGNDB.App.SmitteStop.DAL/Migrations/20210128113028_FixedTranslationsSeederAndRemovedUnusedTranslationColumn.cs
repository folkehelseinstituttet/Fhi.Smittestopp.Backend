using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class FixedTranslationsSeederAndRemovedUnusedTranslationColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityPropertyName",
                table: "Translation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EntityPropertyName",
                table: "Translation",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
