using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class _10_5v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "notes",
                table: "EmployeeDocuments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "filePath",
                table: "EmployeeDocuments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "documentType",
                table: "EmployeeDocuments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "documentUrl",
                table: "EmployeeDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expiryDate",
                table: "EmployeeDocuments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "folderId",
                table: "EmployeeDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "tag",
                table: "EmployeeDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "documentTags",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentTags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employeeDocumentFolders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    employeeId = table.Column<int>(type: "int", nullable: false),
                    folderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    updateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employeeDocumentFolders", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documentTags");

            migrationBuilder.DropTable(
                name: "employeeDocumentFolders");

            migrationBuilder.DropColumn(
                name: "documentUrl",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "expiryDate",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "folderId",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "tag",
                table: "EmployeeDocuments");

            migrationBuilder.AlterColumn<string>(
                name: "notes",
                table: "EmployeeDocuments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "filePath",
                table: "EmployeeDocuments",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "documentType",
                table: "EmployeeDocuments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
