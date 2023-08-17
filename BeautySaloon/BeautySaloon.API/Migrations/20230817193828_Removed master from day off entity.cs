using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.API.Migrations
{
    /// <inheritdoc />
    public partial class Removedmasterfromdayoffentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayOff_Masters_MasterId",
                table: "DayOff");

            migrationBuilder.DropIndex(
                name: "IX_DayOff_MasterId",
                table: "DayOff");

            migrationBuilder.DropColumn(
                name: "MasterId",
                table: "DayOff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MasterId",
                table: "DayOff",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DayOff_MasterId",
                table: "DayOff",
                column: "MasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_DayOff_Masters_MasterId",
                table: "DayOff",
                column: "MasterId",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
