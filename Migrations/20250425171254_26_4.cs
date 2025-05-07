using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class _26_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractAddendums",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "voucher_code",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "ApprovalUser",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "CancelReason",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "detail_note",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "voucher_date",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "voucher_number",
                table: "ContractAddendums");

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "ContractAddendums",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "ContractAddendums",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "ContractAddendums",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractAddendums",
                table: "ContractAddendums",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractAddendums",
                table: "ContractAddendums");

            migrationBuilder.DropColumn(
                name: "status",
                table: "ContractAddendums");

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "ContractAddendums",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "ContractAddendums",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "voucher_code",
                table: "ContractAddendums",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "ApprovalStatus",
                table: "ContractAddendums",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalUser",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentID",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "detail_note",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_date",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_number",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractAddendums",
                table: "ContractAddendums",
                column: "voucher_code");
        }
    }
}
