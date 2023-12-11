using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyAnimeVault.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class updatedForeignKeyIsRequiredToFalseForPostersAndStartSeasons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Posters_PosterId",
                table: "Animes");

            migrationBuilder.DropForeignKey(
                name: "FK_Animes_StartSeasons_StartSeasonId",
                table: "Animes");

            migrationBuilder.AlterColumn<string>(
                name: "WatchStatus",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StartSeasonId",
                table: "Animes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PosterId",
                table: "Animes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Posters_PosterId",
                table: "Animes",
                column: "PosterId",
                principalTable: "Posters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_StartSeasons_StartSeasonId",
                table: "Animes",
                column: "StartSeasonId",
                principalTable: "StartSeasons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animes_Posters_PosterId",
                table: "Animes");

            migrationBuilder.DropForeignKey(
                name: "FK_Animes_StartSeasons_StartSeasonId",
                table: "Animes");

            migrationBuilder.AlterColumn<string>(
                name: "WatchStatus",
                table: "Animes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "StartSeasonId",
                table: "Animes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PosterId",
                table: "Animes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_Posters_PosterId",
                table: "Animes",
                column: "PosterId",
                principalTable: "Posters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Animes_StartSeasons_StartSeasonId",
                table: "Animes",
                column: "StartSeasonId",
                principalTable: "StartSeasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
