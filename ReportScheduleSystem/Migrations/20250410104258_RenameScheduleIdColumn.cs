using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportScheduleSystem.Migrations
{
    /// <inheritdoc />
    public partial class RenameScheduleIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Schedules",
                newName: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScheduleId",
                table: "Schedules",
                newName: "Id");
        }
    }
}
