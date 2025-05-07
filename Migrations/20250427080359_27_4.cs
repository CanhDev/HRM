using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class _27_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Terminations",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "voucher_code",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "ApprovalUser",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "CancelReason",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "DepartmentID",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "RejectReason",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "detail_note",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "voucher_date",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "voucher_number",
                table: "Terminations");

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "Terminations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "Terminations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "Terminations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Terminations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "ContractHistories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "ContractAddendums",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Terminations",
                table: "Terminations",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Terminations",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "Terminations");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Terminations");

            migrationBuilder.AlterColumn<string>(
                name: "updateBy",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updateAt",
                table: "Terminations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "Terminations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "createBy",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "voucher_code",
                table: "Terminations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<short>(
                name: "ApprovalStatus",
                table: "Terminations",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "ApprovalUser",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CancelReason",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentID",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RejectReason",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "detail_note",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_date",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "voucher_number",
                table: "Terminations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ContractId",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ContractId",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Terminations",
                table: "Terminations",
                column: "voucher_code");
        }
    }
}
