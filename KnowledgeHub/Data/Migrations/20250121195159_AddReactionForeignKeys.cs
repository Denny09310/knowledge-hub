using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KnowledgeHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReactionForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reactions_users_user_id",
                table: "reactions");

            migrationBuilder.DropIndex(
                name: "ix_reactions_user_id",
                table: "reactions");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "reactions",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "reactions",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "reactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "reactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.CreateIndex(
                name: "ix_reactions_user_id",
                table: "reactions",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_reactions_users_user_id",
                table: "reactions",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}