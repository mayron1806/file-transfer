using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiveStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receive_Received",
                table: "Transfers");

            migrationBuilder.AddColumn<int>(
                name: "Receive_Status",
                table: "Transfers",
                type: "integer",
                nullable: true,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receive_Status",
                table: "Transfers");

            migrationBuilder.AddColumn<bool>(
                name: "Receive_Received",
                table: "Transfers",
                type: "boolean",
                nullable: true,
                defaultValue: false);
        }
    }
}
