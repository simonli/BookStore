using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BookStore.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_keys",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(127)", nullable: false),
                    MaxId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_app_keys", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Author = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    AuthorIntroduction = table.Column<string>(type: "longtext", nullable: true),
                    BookCatelog = table.Column<string>(type: "longtext", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DoubanId = table.Column<int>(type: "int", nullable: false),
                    DoubanRatingPeople = table.Column<int>(type: "int", nullable: false),
                    DoubanRatingScore = table.Column<float>(type: "float", nullable: false),
                    DoubanUrl = table.Column<string>(type: "longtext", nullable: true),
                    Introduction = table.Column<string>(type: "longtext", nullable: true),
                    IsDelete = table.Column<int>(type: "int", nullable: false),
                    Isbn = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Logo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Publisher = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    Title = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Translator = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Avatar = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    IsDelete = table.Column<int>(type: "int", nullable: false),
                    LoginCount = table.Column<int>(type: "int", nullable: false),
                    LoginIp = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    LoginTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Password = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    PointCount = table.Column<int>(type: "int", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "book_tag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book_tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_book_tag_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_book_tag_tags_TagId",
                        column: x => x.TagId,
                        principalTable: "tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "book_editions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BookId = table.Column<long>(type: "bigint", nullable: true),
                    CheckSum = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DownloadCount = table.Column<int>(type: "int", nullable: false),
                    FavoriteCount = table.Column<int>(type: "int", nullable: false),
                    Filename = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    Filesize = table.Column<long>(type: "bigint", nullable: false),
                    OriginalFilename = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    PushCount = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book_editions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_book_editions_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_book_editions_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "push_settings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsDefault = table.Column<int>(type: "int", nullable: false),
                    PushEmail = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_push_settings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_push_settings_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "action_logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    BookEditionId = table.Column<long>(type: "bigint", nullable: true),
                    CheckinPoint = table.Column<int>(type: "int", nullable: false),
                    CheckinTotalPoint = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PushEmail = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    PushFromPlatform = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    PushStatus = table.Column<int>(type: "int", nullable: false),
                    PushUseTime = table.Column<int>(type: "int", nullable: false),
                    Taxonomy = table.Column<int>(type: "int", nullable: false),
                    UserAgent = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_action_logs_book_editions_BookEditionId",
                        column: x => x.BookEditionId,
                        principalTable: "book_editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_action_logs_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "book_edition_comments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    AtUserId = table.Column<long>(type: "bigint", nullable: true),
                    BookEditionId = table.Column<long>(type: "bigint", nullable: true),
                    Comment = table.Column<string>(type: "longtext", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book_edition_comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_book_edition_comments_users_AtUserId",
                        column: x => x.AtUserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_book_edition_comments_book_editions_BookEditionId",
                        column: x => x.BookEditionId,
                        principalTable: "book_editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_book_edition_comments_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_point_logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ActionLogId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Point = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_point_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_point_logs_action_logs_ActionLogId",
                        column: x => x.ActionLogId,
                        principalTable: "action_logs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_point_logs_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_action_logs_BookEditionId",
                table: "action_logs",
                column: "BookEditionId");

            migrationBuilder.CreateIndex(
                name: "IX_action_logs_UserId",
                table: "action_logs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_app_keys_Name",
                table: "app_keys",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_book_edition_comments_AtUserId",
                table: "book_edition_comments",
                column: "AtUserId");

            migrationBuilder.CreateIndex(
                name: "IX_book_edition_comments_BookEditionId",
                table: "book_edition_comments",
                column: "BookEditionId");

            migrationBuilder.CreateIndex(
                name: "IX_book_edition_comments_UserId",
                table: "book_edition_comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_book_editions_BookId",
                table: "book_editions",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_book_editions_UserId",
                table: "book_editions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_book_tag_BookId",
                table: "book_tag",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_book_tag_TagId",
                table: "book_tag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_books_Author",
                table: "books",
                column: "Author");

            migrationBuilder.CreateIndex(
                name: "IX_books_Isbn",
                table: "books",
                column: "Isbn");

            migrationBuilder.CreateIndex(
                name: "IX_books_Publisher",
                table: "books",
                column: "Publisher");

            migrationBuilder.CreateIndex(
                name: "IX_books_Title",
                table: "books",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_push_settings_UserId",
                table: "push_settings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_point_logs_ActionLogId",
                table: "user_point_logs",
                column: "ActionLogId");

            migrationBuilder.CreateIndex(
                name: "IX_user_point_logs_UserId",
                table: "user_point_logs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_Username",
                table: "users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "app_keys");

            migrationBuilder.DropTable(
                name: "book_edition_comments");

            migrationBuilder.DropTable(
                name: "book_tag");

            migrationBuilder.DropTable(
                name: "push_settings");

            migrationBuilder.DropTable(
                name: "user_point_logs");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "action_logs");

            migrationBuilder.DropTable(
                name: "book_editions");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
