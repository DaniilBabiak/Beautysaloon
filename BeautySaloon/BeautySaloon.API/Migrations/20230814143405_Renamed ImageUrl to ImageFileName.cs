using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.API.Migrations
{
    /// <inheritdoc />
    public partial class RenamedImageUrltoImageFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ServiceCategories",
                newName: "ImageFileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageFileName",
                table: "ServiceCategories",
                newName: "ImageUrl");
        }
    }
}
