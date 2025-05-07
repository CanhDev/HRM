using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class _26_4v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmploymentContracts",
                table: "EmploymentContracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractHistories",
                table: "ContractHistories");

            migrationBuilder.DropColumn(
                name: "voucher_code",
                table: "EmploymentContracts");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "EmploymentContracts");

            migrationBuilder.DropColumn(
                name: "ApprovalUser",
                table: "EmploymentContracts");

            migrationBuilder.DropColumn(
                name: "CancelReason",
                table: "EmploymentContracts");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "EmploymentContracts");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "EmploymentContracts");

            migrationBuilder.DropColumn(
                name: "detail_note",
                table: "EmploymentContracts");

            migrationBuilder.DropColumn(
                name: "voucher_date",
                table: "EmploymentContracts");

            migrationBuilder.DropColumn(
                name: "voucher_number",
                table: "EmploymentContracts");

            migrationBuilder.DropColumn(
                name: "voucher_code",
                table: "ContractHistories");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "ContractHistories");

            migrationBuilder.DropColumn(
                name: "ApprovalUser",
                table: "ContractHistories");

            migrationBuilder.DropColumn(
                name: "CancelReason",
                table: "ContractHistories");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "ContractHistories");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "ContractHistories");

            migrationBuilder.DropColumn(
                name: "detail_note",
                table: "ContractHistories");

            migrationBuilder.DropColumn(
                name: "voucher_date",
                table: "ContractHistories");

            migrationBuilder.DropColumn(
                name: "voucher_number",
                table: "ContractHistories");

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "EmploymentContracts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "EmploymentContracts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "EmploymentContracts",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "ContractHistories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "ContractHistories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "ContractHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmploymentContracts",
                table: "EmploymentContracts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractHistories",
                table: "ContractHistories",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmploymentContracts",
                table: "EmploymentContracts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractHistories",
                table: "ContractHistories");

            migrationBuilder.DropColumn(
                name: "status",
                table: "EmploymentContracts");

            migrationBuilder.DropColumn(
                name: "status",
                table: "ContractHistories");

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "EmploymentContracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "EmploymentContracts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "voucher_code",
                table: "EmploymentContracts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "ApprovalStatus",
                table: "EmploymentContracts",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalUser",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentID",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "detail_note",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_date",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_number",
                table: "EmploymentContracts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "ContractHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "ContractHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "voucher_code",
                table: "ContractHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "ApprovalStatus",
                table: "ContractHistories",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalUser",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentID",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "detail_note",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_date",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_number",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmploymentContracts",
                table: "EmploymentContracts",
                column: "voucher_code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractHistories",
                table: "ContractHistories",
                column: "voucher_code");
        }
    }
}
