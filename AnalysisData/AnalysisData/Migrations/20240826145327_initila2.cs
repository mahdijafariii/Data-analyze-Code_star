using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalysisData.Migrations
{
    /// <inheritdoc />
    public partial class initila2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "UploadDatas");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "UploadDatas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UploadDatas_CategoryId",
                table: "UploadDatas",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadDatas_Categories_CategoryId",
                table: "UploadDatas",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadDatas_Categories_CategoryId",
                table: "UploadDatas");

            migrationBuilder.DropIndex(
                name: "IX_UploadDatas_CategoryId",
                table: "UploadDatas");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "UploadDatas");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "UploadDatas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
