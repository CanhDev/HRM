using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class _10_5v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeDocumentFolderid",
                table: "EmployeeDocuments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocuments_EmployeeDocumentFolderid",
                table: "EmployeeDocuments",
                column: "EmployeeDocumentFolderid");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeDocuments_employeeDocumentFolders_EmployeeDocumentFolderid",
                table: "EmployeeDocuments",
                column: "EmployeeDocumentFolderid",
                principalTable: "employeeDocumentFolders",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeDocuments_employeeDocumentFolders_EmployeeDocumentFolderid",
                table: "EmployeeDocuments");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeDocuments_EmployeeDocumentFolderid",
                table: "EmployeeDocuments");

            migrationBuilder.DropColumn(
                name: "EmployeeDocumentFolderid",
                table: "EmployeeDocuments");
        }
    }
}
