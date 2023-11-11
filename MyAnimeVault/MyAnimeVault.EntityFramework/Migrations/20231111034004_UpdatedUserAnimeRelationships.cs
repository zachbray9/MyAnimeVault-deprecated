using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAnimeVault.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserAnimeRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posters_Animes_AnimeId",
                table: "Posters");

            migrationBuilder.DropTable(
                name: "UserUserAnime");

            migrationBuilder.DropIndex(
                name: "IX_Posters_AnimeId",
                table: "Posters");

            migrationBuilder.DropColumn(
                name: "AnimeId",
                table: "Posters");

            migrationBuilder.AddColumn<int>(
                name: "PosterId",
                table: "Animes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Animes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Animes_PosterId",
                table: "Animes",
                column: "PosterId");

            migrationBuilder.CreateIndex(
                name: "IX_Animes_UserId",
                table: "Animes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Posters_PosterId",
                table: "Animes",
                column: "PosterId",
                principalTable: "Posters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Users_UserId",
                table: "Animes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Posters_PosterId",
                table: "Animes");

            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Users_UserId",
                table: "Animes");

            migrationBuilder.DropIndex(
                name: "IX_Animes_PosterId",
                table: "Animes");

            migrationBuilder.DropIndex(
                name: "IX_Animes_UserId",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "PosterId",
                table: "Animes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Animes");

            migrationBuilder.AddColumn<int>(
                name: "AnimeId",
                table: "Posters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserUserAnime",
                columns: table => new
                {
                    AnimesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUserAnime", x => new { x.AnimesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserUserAnime_Animes_AnimesId",
                        column: x => x.AnimesId,
                        principalTable: "Animes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUserAnime_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posters_AnimeId",
                table: "Posters",
                column: "AnimeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserUserAnime_UsersId",
                table: "UserUserAnime",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posters_Animes_AnimeId",
                table: "Posters",
                column: "AnimeId",
                principalTable: "Animes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
