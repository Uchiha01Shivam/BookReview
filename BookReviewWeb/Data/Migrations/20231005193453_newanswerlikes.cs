using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReviewWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class newanswerlikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisLikes",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "IsLiked",
                table: "Community");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "Community");

            migrationBuilder.CreateTable(
                name: "AnsLike",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answerId = table.Column<int>(type: "int", nullable: false),
                    Likes = table.Column<int>(type: "int", nullable: true),
                    DisLikes = table.Column<int>(type: "int", nullable: true),
                    IsLiked = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnsLike", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnsLike_Community_answerId",
                        column: x => x.answerId,
                        principalTable: "Community",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnsLike_answerId",
                table: "AnsLike",
                column: "answerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnsLike");

            migrationBuilder.AddColumn<int>(
                name: "DisLikes",
                table: "Community",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLiked",
                table: "Community",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "Community",
                type: "int",
                nullable: true);
        }
    }
}
