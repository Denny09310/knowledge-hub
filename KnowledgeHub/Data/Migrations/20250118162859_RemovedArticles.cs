using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnowledgeHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedArticles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Users_AuthorId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Articles_ArticleId",
                table: "Reactions");

            migrationBuilder.DropIndex(
                name: "IX_Reactions_ArticleId",
                table: "Reactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Articles",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "TotalReactions",
                table: "Articles");

            migrationBuilder.RenameTable(
                name: "Articles",
                newName: "Article");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Article",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Article",
                newName: "Author");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Article",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Article",
                table: "Article",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Article_UserId",
                table: "Article",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Users_UserId",
                table: "Article",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Users_UserId",
                table: "Article");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Article",
                table: "Article");

            migrationBuilder.DropIndex(
                name: "IX_Article_UserId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Article");

            migrationBuilder.RenameTable(
                name: "Article",
                newName: "Articles");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Articles",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Articles",
                newName: "Content");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Articles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalReactions",
                table: "Articles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articles",
                table: "Articles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ArticleId",
                table: "Reactions",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_AuthorId",
                table: "Articles",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Users_AuthorId",
                table: "Articles",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Articles_ArticleId",
                table: "Reactions",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
