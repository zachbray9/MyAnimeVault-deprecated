using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAnimeVault.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddedAnimeIdToUserAnime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnimeId",
                table: "Animes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimeId",
                table: "Animes");
        }
    }
}
