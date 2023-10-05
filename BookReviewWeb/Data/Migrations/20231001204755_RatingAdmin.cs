using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReviewWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class RatingAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorCredibility",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Illustration",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InGeneral",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OverAll",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhysicalAppearance",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublisherCreibility",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubjectMatter",
                table: "Book",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorCredibility",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Illustration",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "InGeneral",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "OverAll",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "PhysicalAppearance",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "PublisherCreibility",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "SubjectMatter",
                table: "Book");
        }
    }
}
