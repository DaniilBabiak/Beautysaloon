using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.API.Migrations
{
    /// <inheritdoc />
    public partial class DayOffsnowhavescheduleid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayOff_Schedules_ScheduleId",
                table: "DayOff");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId",
                table: "WorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_MasterId",
                table: "Schedules");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "WorkingDays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MasterId",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "DayOff",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_MasterId",
                table: "Schedules",
                column: "MasterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DayOff_Schedules_ScheduleId",
                table: "DayOff",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId",
                table: "WorkingDays",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayOff_Schedules_ScheduleId",
                table: "DayOff");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId",
                table: "WorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_MasterId",
                table: "Schedules");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "WorkingDays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MasterId",
                table: "Schedules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "DayOff",
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
                name: "FK_DayOff_Schedules_ScheduleId",
                table: "DayOff",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId",
                table: "WorkingDays",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");
        }
    }
}
