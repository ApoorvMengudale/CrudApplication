using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crud_Application.Migrations
{
    /// <inheritdoc />
    public partial class AuditChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Audit");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Audit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Audit_UserId",
                table: "Audit",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audit_Users_UserId",
                table: "Audit",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audit_Users_UserId",
                table: "Audit");

            migrationBuilder.DropIndex(
                name: "IX_Audit_UserId",
                table: "Audit");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Audit");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Audit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
