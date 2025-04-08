using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportScheduleSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFileColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileData",
                table: "Reports",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "FileData",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Reports");
        }
    }
}
