using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReviewWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class newonewithlikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisLike",
                table: "discussions");

            migrationBuilder.DropColumn(
                name: "IsLiked",
                table: "discussions");

            migrationBuilder.DropColumn(
                name: "Like",
                table: "discussions");

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscussionId = table.Column<int>(type: "int", nullable: false),
                    IsLiked = table.Column<bool>(type: "bit", nullable: true),
                    Like = table.Column<int>(type: "int", nullable: true),
                    DisLike = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_likes_discussions_DiscussionId",
                        column: x => x.DiscussionId,
                        principalTable: "discussions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_likes_DiscussionId",
                table: "likes",
                column: "DiscussionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.AddColumn<int>(
                name: "DisLike",
                table: "discussions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                table: "discussions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Like",
                table: "discussions",
                type: "int",
                nullable: true);
        }
    }
}
