using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AnalysisData.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttributeEdges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeEdges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttributeNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeNodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    RolePolicy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    ImageURL = table.Column<string>(type: "text", nullable: true),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileUploadedDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UploaderId = table.Column<Guid>(type: "uuid", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploadedDb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileUploadedDb_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileUploadedDb_Users_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NodeFileReferenceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityNodes_FileUploadedDb_NodeFileReferenceId",
                        column: x => x.NodeFileReferenceId,
                        principalTable: "FileUploadedDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFiles_FileUploadedDb_FileId",
                        column: x => x.FileId,
                        principalTable: "FileUploadedDb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityEdges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityIDSource = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityIDTarget = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityEdges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityEdges_EntityNodes_EntityIDSource",
                        column: x => x.EntityIDSource,
                        principalTable: "EntityNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityEdges_EntityNodes_EntityIDTarget",
                        column: x => x.EntityIDTarget,
                        principalTable: "EntityNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValueNodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValueNodes_AttributeNodes_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "AttributeNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValueNodes_EntityNodes_EntityId",
                        column: x => x.EntityId,
                        principalTable: "EntityNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValueEdges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueEdges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValueEdges_AttributeEdges_AttributeId",
                        column: x => x.AttributeId,
                        principalTable: "AttributeEdges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValueEdges_EntityEdges_EntityId",
                        column: x => x.EntityId,
                        principalTable: "EntityEdges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName", "RolePolicy" },
                values: new object[,]
                {
                    { 1, "Admin", "gold" },
                    { 2, "Data-Analyst", "bronze" },
                    { 3, "Data-Manager", "silver" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "ImageURL", "LastName", "Password", "PhoneNumber", "RoleId", "Username" },
                values: new object[] { new Guid("d8a83af5-1202-4307-b852-bd941a331dd0"), "admin@gmail.com", "admin", null, "admin", "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", "09131111111", 1, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityEdges_EntityIDSource",
                table: "EntityEdges",
                column: "EntityIDSource");

            migrationBuilder.CreateIndex(
                name: "IX_EntityEdges_EntityIDTarget",
                table: "EntityEdges",
                column: "EntityIDTarget");

            migrationBuilder.CreateIndex(
                name: "IX_EntityNodes_NodeFileReferenceId",
                table: "EntityNodes",
                column: "NodeFileReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadedDb_CategoryId",
                table: "FileUploadedDb",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FileUploadedDb_UploaderId",
                table: "FileUploadedDb",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserId",
                table: "Tokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_FileId",
                table: "UserFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFiles_UserId",
                table: "UserFiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueEdges_AttributeId",
                table: "ValueEdges",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueEdges_EntityId",
                table: "ValueEdges",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueNodes_AttributeId",
                table: "ValueNodes",
                column: "AttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueNodes_EntityId",
                table: "ValueNodes",
                column: "EntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "UserFiles");

            migrationBuilder.DropTable(
                name: "ValueEdges");

            migrationBuilder.DropTable(
                name: "ValueNodes");

            migrationBuilder.DropTable(
                name: "AttributeEdges");

            migrationBuilder.DropTable(
                name: "EntityEdges");

            migrationBuilder.DropTable(
                name: "AttributeNodes");

            migrationBuilder.DropTable(
                name: "EntityNodes");

            migrationBuilder.DropTable(
                name: "FileUploadedDb");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
