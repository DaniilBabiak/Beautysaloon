using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySaloon.API.Migrations
{
    /// <inheritdoc />
    public partial class Configuredrelationsbetweenscheduleandworkingdaysv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId1",
                table: "WorkingDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkingDays_ScheduleId1",
                table: "WorkingDays");

            migrationBuilder.DropColumn(
                name: "ScheduleId1",
                table: "WorkingDays");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScheduleId1",
                table: "WorkingDays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDays_ScheduleId1",
                table: "WorkingDays",
                column: "ScheduleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingDays_Schedules_ScheduleId1",
                table: "WorkingDays",
                column: "ScheduleId1",
                principalTable: "Schedules",
                principalColumn: "Id");
        }
    }
}
