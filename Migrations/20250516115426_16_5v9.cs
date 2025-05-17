using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class _16_5v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "endDate",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "status",
                table: "LeaveRequests");

            migrationBuilder.RenameColumn(
                name: "departmentID",
                table: "LeaveRequests",
                newName: "departmentId");

            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "LeaveRequests",
                newName: "createDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "departmentId",
                table: "LeaveRequests",
                newName: "departmentID");

            migrationBuilder.RenameColumn(
                name: "createDate",
                table: "LeaveRequests",
                newName: "startDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "endDate",
                table: "LeaveRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "LeaveRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
