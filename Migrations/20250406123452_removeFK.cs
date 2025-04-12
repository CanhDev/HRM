using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class removeFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractAddendums_EmploymentContracts_ContractId",
                table: "ContractAddendums");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractHistories_EmploymentContracts_ContractId",
                table: "ContractHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employees_ManagerID",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Educations_Employees_EmployeeId",
                table: "Educations");

            migrationBuilder.DropForeignKey(
                name: "FK_emergencyContacts_Employees_EmployeeId",
                table: "emergencyContacts");

            migrationBuilder.DropForeignKey(
                name: "FK_employeeLeaveBalances_LeaveTypes_LeaveTypeId",
                table: "employeeLeaveBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_ContractTypes_ContractTypeID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_EmploymentContracts_ContractTypes_ContractTypeID",
                table: "EmploymentContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_leaveBalanceHistories_employeeLeaveBalances_BalanceID",
                table: "leaveBalanceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_workExperiences_Employees_EmployeeId",
                table: "workExperiences");

            migrationBuilder.DropIndex(
                name: "IX_workExperiences_EmployeeId",
                table: "workExperiences");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_leaveBalanceHistories_BalanceID",
                table: "leaveBalanceHistories");

            migrationBuilder.DropIndex(
                name: "IX_EmploymentContracts_ContractTypeID",
                table: "EmploymentContracts");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ContractTypeID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_employeeLeaveBalances_LeaveTypeId",
                table: "employeeLeaveBalances");

            migrationBuilder.DropIndex(
                name: "IX_emergencyContacts_EmployeeId",
                table: "emergencyContacts");

            migrationBuilder.DropIndex(
                name: "IX_Educations_EmployeeId",
                table: "Educations");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ManagerID",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_ContractHistories_ContractId",
                table: "ContractHistories");

            migrationBuilder.DropIndex(
                name: "IX_ContractAddendums_ContractId",
                table: "ContractAddendums");

            migrationBuilder.AlterColumn<string>(
                name: "BalanceID",
                table: "leaveBalanceHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ContractId",
                table: "ContractHistories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ContractId",
                table: "ContractAddendums",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BalanceID",
                table: "leaveBalanceHistories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContractId",
                table: "ContractHistories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContractId",
                table: "ContractAddendums",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_workExperiences_EmployeeId",
                table: "workExperiences",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_leaveBalanceHistories_BalanceID",
                table: "leaveBalanceHistories",
                column: "BalanceID");

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentContracts_ContractTypeID",
                table: "EmploymentContracts",
                column: "ContractTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ContractTypeID",
                table: "Employees",
                column: "ContractTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentID",
                table: "Employees",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_employeeLeaveBalances_LeaveTypeId",
                table: "employeeLeaveBalances",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_emergencyContacts_EmployeeId",
                table: "emergencyContacts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_EmployeeId",
                table: "Educations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ManagerID",
                table: "Departments",
                column: "ManagerID");

            migrationBuilder.CreateIndex(
                name: "IX_ContractHistories_ContractId",
                table: "ContractHistories",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractAddendums_ContractId",
                table: "ContractAddendums",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractAddendums_EmploymentContracts_ContractId",
                table: "ContractAddendums",
                column: "ContractId",
                principalTable: "EmploymentContracts",
                principalColumn: "voucher_code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractHistories_EmploymentContracts_ContractId",
                table: "ContractHistories",
                column: "ContractId",
                principalTable: "EmploymentContracts",
                principalColumn: "voucher_code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employees_ManagerID",
                table: "Departments",
                column: "ManagerID",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Educations_Employees_EmployeeId",
                table: "Educations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_emergencyContacts_Employees_EmployeeId",
                table: "emergencyContacts",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_employeeLeaveBalances_LeaveTypes_LeaveTypeId",
                table: "employeeLeaveBalances",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_ContractTypes_ContractTypeID",
                table: "Employees",
                column: "ContractTypeID",
                principalTable: "ContractTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentID",
                table: "Employees",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmploymentContracts_ContractTypes_ContractTypeID",
                table: "EmploymentContracts",
                column: "ContractTypeID",
                principalTable: "ContractTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_leaveBalanceHistories_employeeLeaveBalances_BalanceID",
                table: "leaveBalanceHistories",
                column: "BalanceID",
                principalTable: "employeeLeaveBalances",
                principalColumn: "voucher_code",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_workExperiences_Employees_EmployeeId",
                table: "workExperiences",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
