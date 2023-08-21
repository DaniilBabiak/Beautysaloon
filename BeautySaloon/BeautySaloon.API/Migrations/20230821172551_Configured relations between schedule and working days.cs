using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.API.Migrations
{
    /// <inheritdoc />
    public partial class Configuredrelationsbetweenscheduleandworkingdays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "Masters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Masters_ScheduleId",
                table: "Masters",
                column: "ScheduleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Masters_Schedules_ScheduleId",
                table: "Masters",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Masters_Schedules_ScheduleId",
                table: "Masters");

            migrationBuilder.DropIndex(
                name: "IX_Masters_ScheduleId",
                table: "Masters");

            migrationBuilder.AddColumn<int>(
                name: "MasterId",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "Masters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
    }
}
