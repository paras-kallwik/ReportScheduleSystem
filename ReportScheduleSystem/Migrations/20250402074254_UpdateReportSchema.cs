using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReportScheduleSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReportSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reports",
                newName: "User_Id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Reports",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Reports",
                newName: "Is_Active");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reports",
                newName: "Created_At");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Reports",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "FileData",
                table: "Reports",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "Reports",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "Reports",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "Reports",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Is_Active",
                table: "Reports",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "Reports",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "FileName",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "FileData",
                table: "Reports",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContentType",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
