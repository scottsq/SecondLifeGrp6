using Microsoft.EntityFrameworkCore.Migrations;

namespace VS_SLG6.Model.Migrations
{
    public partial class CommentRatings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "UserRating",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "ProductRating",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "UserRating");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "ProductRating");
        }
    }
}
