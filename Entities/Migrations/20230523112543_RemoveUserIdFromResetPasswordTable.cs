using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdFromResetPasswordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResetPassword_Users_UserId",
                table: "ResetPassword");

            migrationBuilder.DropIndex(
                name: "IX_ResetPassword_UserId",
                table: "ResetPassword");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ResetPassword");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "ResetPassword",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "ResetPassword");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "ResetPassword",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ResetPassword_UserId",
                table: "ResetPassword",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResetPassword_Users_UserId",
                table: "ResetPassword",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
