using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnalysisData.Migrations
{
    /// <inheritdoc />
    public partial class initial3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploadedDb_Users_UserId",
                table: "FileUploadedDb");

            migrationBuilder.DropIndex(
                name: "IX_FileUploadedDb_UserId",
                table: "FileUploadedDb");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FileUploadedDb");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadedDb_UploaderId",
                table: "FileUploadedDb",
                column: "UploaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploadedDb_Users_UploaderId",
                table: "FileUploadedDb",
                column: "UploaderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploadedDb_Users_UploaderId",
                table: "FileUploadedDb");

            migrationBuilder.DropIndex(
                name: "IX_FileUploadedDb_UploaderId",
                table: "FileUploadedDb");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "FileUploadedDb",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadedDb_UserId",
                table: "FileUploadedDb",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploadedDb_Users_UserId",
                table: "FileUploadedDb",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
