using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransferExpirationAndOrgLimiter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastDayUploadCheck",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "LastDayUploadCount",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "UploadCount",
                table: "Organizations");

            migrationBuilder.AddColumn<bool>(
                name: "Expired",
                table: "Transfers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DayUploadCount",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Expired",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "DayUploadCount",
                table: "Organizations");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDayUploadCheck",
                table: "Organizations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "LastDayUploadCount",
                table: "Organizations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UploadCount",
                table: "Organizations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
