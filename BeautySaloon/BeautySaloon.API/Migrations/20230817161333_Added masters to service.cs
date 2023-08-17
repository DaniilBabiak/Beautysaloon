using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.API.Migrations
{
    /// <inheritdoc />
    public partial class Addedmasterstoservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Masters_MasterId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_MasterId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "MasterId",
                table: "Services");

            migrationBuilder.CreateTable(
                name: "MasterService",
                columns: table => new
                {
                    MastersId = table.Column<int>(type: "int", nullable: false),
                    ServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterService", x => new { x.MastersId, x.ServicesId });
                    table.ForeignKey(
                        name: "FK_MasterService_Masters_MastersId",
                        column: x => x.MastersId,
                        principalTable: "Masters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterService_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterService_ServicesId",
                table: "MasterService",
                column: "ServicesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterService");

            migrationBuilder.AddColumn<int>(
                name: "MasterId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_MasterId",
                table: "Services",
                column: "MasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Masters_MasterId",
                table: "Services",
                column: "MasterId",
                principalTable: "Masters",
                principalColumn: "Id");
        }
    }
}
