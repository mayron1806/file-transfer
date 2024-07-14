using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_Transfer_TransferId",
                table: "File");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Organization_OrganizationId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Member_Users_UserId",
                table: "Member");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_Organization_OrganizationId",
                table: "Transfer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfer",
                table: "Transfer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organization",
                table: "Organization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Member",
                table: "Member");

            migrationBuilder.DropPrimaryKey(
                name: "PK_File",
                table: "File");

            migrationBuilder.RenameTable(
                name: "Transfer",
                newName: "Transfers");

            migrationBuilder.RenameTable(
                name: "Organization",
                newName: "Organizations");

            migrationBuilder.RenameTable(
                name: "Member",
                newName: "Members");

            migrationBuilder.RenameTable(
                name: "File",
                newName: "Files");

            migrationBuilder.RenameIndex(
                name: "IX_Transfer_OrganizationId",
                table: "Transfers",
                newName: "IX_Transfers_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfer_Key",
                table: "Transfers",
                newName: "IX_Transfers_Key");

            migrationBuilder.RenameIndex(
                name: "IX_Member_OrganizationId",
                table: "Members",
                newName: "IX_Members_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_File_TransferId",
                table: "Files",
                newName: "IX_Files_TransferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfers",
                table: "Transfers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                columns: new[] { "UserId", "OrganizationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Files",
                table: "Files",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Transfers_TransferId",
                table: "Files",
                column: "TransferId",
                principalTable: "Transfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Organizations_OrganizationId",
                table: "Members",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Users_UserId",
                table: "Members",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Organizations_OrganizationId",
                table: "Transfers",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Transfers_TransferId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Organizations_OrganizationId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Users_UserId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Organizations_OrganizationId",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfers",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Files",
                table: "Files");

            migrationBuilder.RenameTable(
                name: "Transfers",
                newName: "Transfer");

            migrationBuilder.RenameTable(
                name: "Organizations",
                newName: "Organization");

            migrationBuilder.RenameTable(
                name: "Members",
                newName: "Member");

            migrationBuilder.RenameTable(
                name: "Files",
                newName: "File");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_OrganizationId",
                table: "Transfer",
                newName: "IX_Transfer_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_Key",
                table: "Transfer",
                newName: "IX_Transfer_Key");

            migrationBuilder.RenameIndex(
                name: "IX_Members_OrganizationId",
                table: "Member",
                newName: "IX_Member_OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Files_TransferId",
                table: "File",
                newName: "IX_File_TransferId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfer",
                table: "Transfer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organization",
                table: "Organization",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Member",
                table: "Member",
                columns: new[] { "UserId", "OrganizationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_File",
                table: "File",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_File_Transfer_TransferId",
                table: "File",
                column: "TransferId",
                principalTable: "Transfer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Organization_OrganizationId",
                table: "Member",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Users_UserId",
                table: "Member",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_Organization_OrganizationId",
                table: "Transfer",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
