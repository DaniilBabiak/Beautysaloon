using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.API.Migrations
{
    /// <inheritdoc />
    public partial class Configuredrelationsbetweenscheduleandworkingdaysv6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Masters_Schedules_ScheduleId",
                table: "Masters");

            migrationBuilder.DropIndex(
                name: "IX_Masters_ScheduleId",
                table: "Masters");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Masters");

            migrationBuilder.AddColumn<int>(
                name: "MasterId",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_MasterId",
                table: "Schedules",
                column: "MasterId",
                unique: true,
                filter: "[MasterId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Masters_MasterId",
                table: "Schedules",
                column: "MasterId",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Masters_MasterId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_MasterId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "MasterId",
                table: "Schedules");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Masters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Masters_ScheduleId",
                table: "Masters",
                column: "ScheduleId",
                unique: true,
                filter: "[ScheduleId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Masters_Schedules_ScheduleId",
                table: "Masters",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
