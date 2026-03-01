using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechClubWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Banners",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Banners");
        }
    }
}
