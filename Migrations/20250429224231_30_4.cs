using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class _30_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_leaveBalanceHistories",
                table: "leaveBalanceHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employeeLeaveBalances",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "LeaveRequestCode",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "voucher_number",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "voucher_code",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "ApprovalUser",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "BalanceID",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "CancelReason",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "ChangeType",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "NewBalance",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "OldBalance",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "detail_note",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "voucher_date",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "voucher_number",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "voucher_code",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "ApprovalUser",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "CancelReason",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "LeaveTypeId",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "detail_note",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "voucher_date",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "voucher_number",
                table: "employeeLeaveBalances");

            migrationBuilder.RenameColumn(
                name: "year",
                table: "employeeLeaveBalances",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "CarriedDays",
                table: "employeeLeaveBalances",
                newName: "Maxday_month");

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "leaveBalanceHistories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "leaveBalanceHistories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ChangeBy",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "leaveBalanceHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeaveTypeId",
                table: "leaveBalanceHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WordCode",
                table: "leaveBalanceHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "leaveBalanceHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "employeeLeaveBalances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "employeeLeaveBalances",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_leaveBalanceHistories",
                table: "leaveBalanceHistories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employeeLeaveBalances",
                table: "employeeLeaveBalances",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_leaveBalanceHistories",
                table: "leaveBalanceHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employeeLeaveBalances",
                table: "employeeLeaveBalances");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "LeaveTypeId",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "WordCode",
                table: "leaveBalanceHistories");

            migrationBuilder.DropColumn(
                name: "status",
                table: "leaveBalanceHistories");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "employeeLeaveBalances",
                newName: "year");

            migrationBuilder.RenameColumn(
                name: "Maxday_month",
                table: "employeeLeaveBalances",
                newName: "CarriedDays");

            migrationBuilder.AddColumn<string>(
                name: "LeaveRequestCode",
                table: "LeaveRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LeaveTypeId",
                table: "LeaveRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "LeaveRequests",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_number",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "leaveBalanceHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "leaveBalanceHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChangeBy",
                table: "leaveBalanceHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "voucher_code",
                table: "leaveBalanceHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "ApprovalStatus",
                table: "leaveBalanceHistories",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalUser",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BalanceID",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChangeType",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentID",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "NewBalance",
                table: "leaveBalanceHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OldBalance",
                table: "leaveBalanceHistories",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "detail_note",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_date",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_number",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "employeeLeaveBalances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "employeeLeaveBalances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "voucher_code",
                table: "employeeLeaveBalances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "ApprovalStatus",
                table: "employeeLeaveBalances",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalUser",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentID",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "employeeLeaveBalances",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "LeaveTypeId",
                table: "employeeLeaveBalances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "detail_note",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_date",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_number",
                table: "employeeLeaveBalances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_leaveBalanceHistories",
                table: "leaveBalanceHistories",
                column: "voucher_code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employeeLeaveBalances",
                table: "employeeLeaveBalances",
                column: "voucher_code");
        }
    }
}
