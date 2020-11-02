using Microsoft.EntityFrameworkCore.Migrations;

namespace DIGNDB.App.SmitteStop.DAL.Migrations
{
    public partial class ChangeSettingNameForGatewayUploadState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Setting] SET [Setting].[Key] = 'GatewaySyncState_UpdateState' WHERE [Setting].[Key] = 'GatewaySyncState_SerializedState'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [Setting] SET [Setting].[Key] = 'GatewaySyncState_SerializedState' WHERE [Setting].[Key] = 'GatewaySyncState_UpdateState'");
        }
    }
}
