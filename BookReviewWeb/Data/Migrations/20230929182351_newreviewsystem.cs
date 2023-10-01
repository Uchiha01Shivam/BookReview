using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReviewWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class newreviewsystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Added",
                table: "Reviews",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Added",
                table: "Reviews");
        }
    }
}
