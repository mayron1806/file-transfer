using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MaxFilesReceived_QuickDownload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Send_QuickDownload",
                table: "Transfers");

            migrationBuilder.AddColumn<int>(
                name: "Receive_MaxFiles",
                table: "Transfers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receive_MaxFiles",
                table: "Transfers");

            migrationBuilder.AddColumn<bool>(
                name: "Send_QuickDownload",
                table: "Transfers",
                type: "boolean",
                nullable: true,
                defaultValue: false);
        }
    }
}
