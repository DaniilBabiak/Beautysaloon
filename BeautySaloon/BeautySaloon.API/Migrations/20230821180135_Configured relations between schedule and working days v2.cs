using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.API.Migrations
{
    /// <inheritdoc />
    public partial class Configuredrelationsbetweenscheduleandworkingdaysv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Masters_ScheduleId",
                table: "Masters");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId1",
                table: "WorkingDays",
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
                name: "IX_WorkingDays_ScheduleId1",
                table: "WorkingDays",
                column: "ScheduleId1");

            migrationBuilder.CreateIndex(
                name: "IX_Masters_ScheduleId",
                table: "Masters",
                column: "ScheduleId",
                unique: true,
                filter: "[ScheduleId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId1",
                table: "WorkingDays",
                column: "ScheduleId1",
                principalTable: "Schedules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId1",
                table: "WorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkingDays_ScheduleId1",
                table: "WorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_Masters_ScheduleId",
                table: "Masters");

            migrationBuilder.DropColumn(
                name: "ScheduleId1",
                table: "WorkingDays");

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
        }
    }
}
