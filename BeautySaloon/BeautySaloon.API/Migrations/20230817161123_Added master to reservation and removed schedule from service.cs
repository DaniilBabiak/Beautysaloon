using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.API.Migrations
{
    /// <inheritdoc />
    public partial class Addedmastertoreservationandremovedschedulefromservice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Masters_Schedules_ScheduleId",
                table: "Masters");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId",
                table: "WorkingDays");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "WorkingDays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MasterId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "Masters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_MasterId",
                table: "Reservations",
                column: "MasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Masters_Schedules_ScheduleId",
                table: "Masters",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Masters_MasterId",
                table: "Reservations",
                column: "MasterId",
                principalTable: "Masters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId",
                table: "WorkingDays",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Masters_Schedules_ScheduleId",
                table: "Masters");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Masters_MasterId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId",
                table: "WorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_MasterId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "MasterId",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "WorkingDays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Services",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Services",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<int>(
                name: "ScheduleId",
                table: "Masters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Masters_Schedules_ScheduleId",
                table: "Masters",
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
    }
}
