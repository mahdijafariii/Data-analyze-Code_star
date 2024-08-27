using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalysisData.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "FileUploadedDb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FileUploadedDb",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
