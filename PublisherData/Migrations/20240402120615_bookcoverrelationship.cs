using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublisherData.Migrations
{
    /// <inheritdoc />
    public partial class bookcoverrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistCover_Artist_ArtistsArtistId",
                table: "ArtistCover");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistCover_Cover_CoversCoverId",
                table: "ArtistCover");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cover",
                table: "Cover");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artist",
                table: "Artist");

            migrationBuilder.RenameTable(
                name: "Cover",
                newName: "Covers");

            migrationBuilder.RenameTable(
                name: "Artist",
                newName: "Artists");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Covers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Covers",
                table: "Covers",
                column: "CoverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artists",
                table: "Artists",
                column: "ArtistId");

            migrationBuilder.UpdateData(
                table: "Covers",
                keyColumn: "CoverId",
                keyValue: 1,
                column: "BookId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Covers",
                keyColumn: "CoverId",
                keyValue: 2,
                column: "BookId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Covers",
                keyColumn: "CoverId",
                keyValue: 3,
                column: "BookId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Covers_BookId",
                table: "Covers",
                column: "BookId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistCover_Artists_ArtistsArtistId",
                table: "ArtistCover",
                column: "ArtistsArtistId",
                principalTable: "Artists",
                principalColumn: "ArtistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistCover_Covers_CoversCoverId",
                table: "ArtistCover",
                column: "CoversCoverId",
                principalTable: "Covers",
                principalColumn: "CoverId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Covers_Books_BookId",
                table: "Covers",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistCover_Artists_ArtistsArtistId",
                table: "ArtistCover");

            migrationBuilder.DropForeignKey(
                name: "FK_ArtistCover_Covers_CoversCoverId",
                table: "ArtistCover");

            migrationBuilder.DropForeignKey(
                name: "FK_Covers_Books_BookId",
                table: "Covers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Covers",
                table: "Covers");

            migrationBuilder.DropIndex(
                name: "IX_Covers_BookId",
                table: "Covers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artists",
                table: "Artists");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Covers");

            migrationBuilder.RenameTable(
                name: "Covers",
                newName: "Cover");

            migrationBuilder.RenameTable(
                name: "Artists",
                newName: "Artist");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cover",
                table: "Cover",
                column: "CoverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artist",
                table: "Artist",
                column: "ArtistId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistCover_Artist_ArtistsArtistId",
                table: "ArtistCover",
                column: "ArtistsArtistId",
                principalTable: "Artist",
                principalColumn: "ArtistId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistCover_Cover_CoversCoverId",
                table: "ArtistCover",
                column: "CoversCoverId",
                principalTable: "Cover",
                principalColumn: "CoverId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
