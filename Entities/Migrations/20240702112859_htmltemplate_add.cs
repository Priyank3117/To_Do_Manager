using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class htmltemplate_add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HtmlTemplate",
                columns: table => new
                {
                    HtmlTemplateId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HtmlTemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeSheetInputLogId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HtmlTemplate", x => x.HtmlTemplateId);
                    table.ForeignKey(
                        name: "FK_HtmlTemplate_TimeSheetInputLogs_TimeSheetInputLogId",
                        column: x => x.TimeSheetInputLogId,
                        principalTable: "TimeSheetInputLogs",
                        principalColumn: "TimeSheetInputLogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HtmlTemplate_TimeSheetInputLogId",
                table: "HtmlTemplate",
                column: "TimeSheetInputLogId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HtmlTemplate");
        }
    }
}
