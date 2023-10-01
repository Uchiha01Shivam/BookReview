using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReviewWeb.Data.Migrations
{
    /// <inheritdoc />
    public partial class newcomment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Reviews",
                newName: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Reviews",
                newName: "UserName");
        }
    }
}
