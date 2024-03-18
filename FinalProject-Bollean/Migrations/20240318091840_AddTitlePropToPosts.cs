using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject_Bollean.Migrations
{
    /// <inheritdoc />
    public partial class AddTitlePropToPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "Posts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "Posts");
        }
    }
}
