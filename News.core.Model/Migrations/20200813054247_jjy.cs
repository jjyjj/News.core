using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace News.core.Model.Migrations
{
    public partial class jjy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: true),
                    IsRemove = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Adress = table.Column<string>(nullable: true),
                    Birthday = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    ImgUrl = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: true),
                    IsRemove = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Focus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: true),
                    IsRemove = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    FocusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Focus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Focus_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: true),
                    IsRemove = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    BrowseCOunt = table.Column<int>(nullable: false),
                    LastChangTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: true),
                    IsRemove = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    NewsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewsToCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: true),
                    IsRemove = table.Column<int>(nullable: true),
                    NewsId = table.Column<int>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsToCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsToCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NewsToCategory_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentsChild",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: true),
                    IsRemove = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    NewsId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    CommentsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsChild", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentsChild_Comments_CommentsId",
                        column: x => x.CommentsId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentsChild_News_NewsId",
                        column: x => x.NewsId,
                        principalTable: "News",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentsChild_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_UserId",
                table: "Category",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_NewsId",
                table: "Comments",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsChild_CommentsId",
                table: "CommentsChild",
                column: "CommentsId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsChild_NewsId",
                table: "CommentsChild",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsChild_UserId",
                table: "CommentsChild",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Focus_UserId",
                table: "Focus",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_News_UserId",
                table: "News",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsToCategory_CategoryId",
                table: "NewsToCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsToCategory_NewsId",
                table: "NewsToCategory",
                column: "NewsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentsChild");

            migrationBuilder.DropTable(
                name: "Focus");

            migrationBuilder.DropTable(
                name: "NewsToCategory");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
