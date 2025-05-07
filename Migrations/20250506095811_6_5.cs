using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Migrations
{
    /// <inheritdoc />
    public partial class _6_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "workExperiences",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "workExperiences",
                newName: "position");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "workExperiences",
                newName: "endDate");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "workExperiences",
                newName: "employeeId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "workExperiences",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "workExperiences",
                newName: "companyName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "workExperiences",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "TerminationType",
                table: "Terminations",
                newName: "terminationType");

            migrationBuilder.RenameColumn(
                name: "TerminationDate",
                table: "Terminations",
                newName: "terminationDate");

            migrationBuilder.RenameColumn(
                name: "TerminationCode",
                table: "Terminations",
                newName: "terminationCode");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "Terminations",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Terminations",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Terminations",
                newName: "employeeId");

            migrationBuilder.RenameColumn(
                name: "ContractId",
                table: "Terminations",
                newName: "contractId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Terminations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sys_Dmtts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PositionName",
                table: "Positions",
                newName: "positionName");

            migrationBuilder.RenameColumn(
                name: "PositionCode",
                table: "Positions",
                newName: "positionCode");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Positions",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Positions",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_Id",
                table: "Positions",
                newName: "IX_Positions_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ListOptions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "MaxDaysAllowed",
                table: "LeaveTypes",
                newName: "maxDaysAllowed");

            migrationBuilder.RenameColumn(
                name: "MaxCarryForwardDays",
                table: "LeaveTypes",
                newName: "maxCarryForwardDays");

            migrationBuilder.RenameColumn(
                name: "LeaveTypeName",
                table: "LeaveTypes",
                newName: "leaveTypeName");

            migrationBuilder.RenameColumn(
                name: "IsPaid",
                table: "LeaveTypes",
                newName: "isPaid");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "LeaveTypes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "CarryForward",
                table: "LeaveTypes",
                newName: "carryForward");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LeaveTypes",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveTypes_Id",
                table: "LeaveTypes",
                newName: "IX_LeaveTypes_id");

            migrationBuilder.RenameColumn(
                name: "TotalDays",
                table: "LeaveRequests",
                newName: "totalDays");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "LeaveRequests",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "RejectReason",
                table: "LeaveRequests",
                newName: "rejectReason");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "LeaveRequests",
                newName: "endDate");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "LeaveRequests",
                newName: "employeeId");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                table: "LeaveRequests",
                newName: "departmentID");

            migrationBuilder.RenameColumn(
                name: "CancelReason",
                table: "LeaveRequests",
                newName: "cancelReason");

            migrationBuilder.RenameColumn(
                name: "ApprovalUser",
                table: "LeaveRequests",
                newName: "approvalUser");

            migrationBuilder.RenameColumn(
                name: "ApprovalStatus",
                table: "LeaveRequests",
                newName: "approvalStatus");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LeaveRequests",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequests_Id",
                table: "LeaveRequests",
                newName: "IX_LeaveRequests_id");

            migrationBuilder.RenameColumn(
                name: "WorkCode",
                table: "leaveRequest_Details",
                newName: "workCode");

            migrationBuilder.RenameColumn(
                name: "TotalDays",
                table: "leaveRequest_Details",
                newName: "totalDays");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "leaveRequest_Details",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "leaveRequest_Details",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "LeaveType",
                table: "leaveRequest_Details",
                newName: "leaveType");

            migrationBuilder.RenameColumn(
                name: "LeaveRequestId",
                table: "leaveRequest_Details",
                newName: "leaveRequestId");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "leaveRequest_Details",
                newName: "endDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "leaveRequest_Details",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "leaveBalanceHistories",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "leaveBalanceHistories",
                newName: "employeeId");

            migrationBuilder.RenameColumn(
                name: "DaysChanged",
                table: "leaveBalanceHistories",
                newName: "daysChanged");

            migrationBuilder.RenameColumn(
                name: "ChangeDate",
                table: "leaveBalanceHistories",
                newName: "changeDate");

            migrationBuilder.RenameColumn(
                name: "ChangeBy",
                table: "leaveBalanceHistories",
                newName: "changeBy");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "leaveBalanceHistories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkingTime",
                table: "EmploymentContracts",
                newName: "workingTime");

            migrationBuilder.RenameColumn(
                name: "Terms",
                table: "EmploymentContracts",
                newName: "terms");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "EmploymentContracts",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "SignedDate",
                table: "EmploymentContracts",
                newName: "signedDate");

            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "EmploymentContracts",
                newName: "salary");

            migrationBuilder.RenameColumn(
                name: "PositionID",
                table: "EmploymentContracts",
                newName: "positionId");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "EmploymentContracts",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "JobDescription",
                table: "EmploymentContracts",
                newName: "jobDescription");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "EmploymentContracts",
                newName: "filePath");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "EmploymentContracts",
                newName: "endDate");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "EmploymentContracts",
                newName: "employeeId");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                table: "EmploymentContracts",
                newName: "departmentId");

            migrationBuilder.RenameColumn(
                name: "ContractTypeID",
                table: "EmploymentContracts",
                newName: "contractTypeId");

            migrationBuilder.RenameColumn(
                name: "ContractCode",
                table: "EmploymentContracts",
                newName: "contractCode");

            migrationBuilder.RenameColumn(
                name: "Benefits",
                table: "EmploymentContracts",
                newName: "benefits");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EmploymentContracts",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_EmploymentContracts_Id",
                table: "EmploymentContracts",
                newName: "IX_EmploymentContracts_id");

            migrationBuilder.RenameColumn(
                name: "TaxCode",
                table: "Employees",
                newName: "taxCode");

            migrationBuilder.RenameColumn(
                name: "PositionID",
                table: "Employees",
                newName: "positionId");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Employees",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "NetSalary",
                table: "Employees",
                newName: "netSalary");

            migrationBuilder.RenameColumn(
                name: "JoinDate",
                table: "Employees",
                newName: "joinDate");

            migrationBuilder.RenameColumn(
                name: "IDCardNumber",
                table: "Employees",
                newName: "idCardNumber");

            migrationBuilder.RenameColumn(
                name: "IDCardIssuePlacec",
                table: "Employees",
                newName: "idCardIssuePlacec");

            migrationBuilder.RenameColumn(
                name: "IDCardIssueDate",
                table: "Employees",
                newName: "idCardIssueDate");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Employees",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Employees",
                newName: "fullName");

            migrationBuilder.RenameColumn(
                name: "EmployeeCode",
                table: "Employees",
                newName: "employeeCode");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Employees",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                table: "Employees",
                newName: "departmentId");

            migrationBuilder.RenameColumn(
                name: "DOB",
                table: "Employees",
                newName: "dob");

            migrationBuilder.RenameColumn(
                name: "ContractTypeID",
                table: "Employees",
                newName: "contractTypeId");

            migrationBuilder.RenameColumn(
                name: "BankName",
                table: "Employees",
                newName: "bankName");

            migrationBuilder.RenameColumn(
                name: "BankBranch",
                table: "Employees",
                newName: "bankBranch");

            migrationBuilder.RenameColumn(
                name: "BankAccountNumber",
                table: "Employees",
                newName: "bankAccountNumber");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Employees",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Employees",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_Id",
                table: "Employees",
                newName: "IX_Employees_id");

            migrationBuilder.RenameColumn(
                name: "UsedDays",
                table: "employeeLeaveBalances",
                newName: "usedDays");

            migrationBuilder.RenameColumn(
                name: "TotalDays",
                table: "employeeLeaveBalances",
                newName: "totalDays");

            migrationBuilder.RenameColumn(
                name: "RemainingDays",
                table: "employeeLeaveBalances",
                newName: "remainingDays");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "employeeLeaveBalances",
                newName: "employeeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "employeeLeaveBalances",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UsedDays_month",
                table: "employeeLeaveBalances",
                newName: "usedDaysMonth");

            migrationBuilder.RenameColumn(
                name: "RemainingDays_month",
                table: "employeeLeaveBalances",
                newName: "remainingDaysMonth");

            migrationBuilder.RenameColumn(
                name: "Maxday_month",
                table: "employeeLeaveBalances",
                newName: "maxDayMonth");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "EmployeeDocuments",
                newName: "notes");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "EmployeeDocuments",
                newName: "filePath");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "EmployeeDocuments",
                newName: "employeeId");

            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "EmployeeDocuments",
                newName: "documentType");

            migrationBuilder.RenameColumn(
                name: "DocumentName",
                table: "EmployeeDocuments",
                newName: "documentName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EmployeeDocuments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Relationship",
                table: "emergencyContacts",
                newName: "relationship");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "emergencyContacts",
                newName: "phoneNumber");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "emergencyContacts",
                newName: "fullName");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "emergencyContacts",
                newName: "employeeId");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "emergencyContacts",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "emergencyContacts",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Educations",
                newName: "startDate");

            migrationBuilder.RenameColumn(
                name: "School",
                table: "Educations",
                newName: "school");

            migrationBuilder.RenameColumn(
                name: "Major",
                table: "Educations",
                newName: "major");

            migrationBuilder.RenameColumn(
                name: "GPA",
                table: "Educations",
                newName: "gpa");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Educations",
                newName: "endDate");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Educations",
                newName: "employeeId");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Educations",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Degree",
                table: "Educations",
                newName: "degree");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Educations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ManagerID",
                table: "Departments",
                newName: "managerID");

            migrationBuilder.RenameColumn(
                name: "DepartmentName",
                table: "Departments",
                newName: "departmentName");

            migrationBuilder.RenameColumn(
                name: "DepartmentCode",
                table: "Departments",
                newName: "departmentCode");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Departments",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_Id",
                table: "Departments",
                newName: "IX_Departments_id");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ContractTypes",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "ContractTypeName",
                table: "ContractTypes",
                newName: "contractTypeName");

            migrationBuilder.RenameColumn(
                name: "ContractTypeCode",
                table: "ContractTypes",
                newName: "contractTypeCode");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ContractTypes",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_ContractTypes_Id",
                table: "ContractTypes",
                newName: "IX_ContractTypes_id");

            migrationBuilder.RenameColumn(
                name: "OldValue",
                table: "ContractHistories",
                newName: "oldValue");

            migrationBuilder.RenameColumn(
                name: "NewValue",
                table: "ContractHistories",
                newName: "newValue");

            migrationBuilder.RenameColumn(
                name: "Desciption",
                table: "ContractHistories",
                newName: "desciption");

            migrationBuilder.RenameColumn(
                name: "ContractId",
                table: "ContractHistories",
                newName: "contractId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ContractHistories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "ContractAddendums",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "ContractAddendums",
                newName: "salary");

            migrationBuilder.RenameColumn(
                name: "PositionID",
                table: "ContractAddendums",
                newName: "positionID");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "ContractAddendums",
                newName: "endDate");

            migrationBuilder.RenameColumn(
                name: "EffectiveDate",
                table: "ContractAddendums",
                newName: "effectiveDate");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                table: "ContractAddendums",
                newName: "departmentID");

            migrationBuilder.RenameColumn(
                name: "ContractId",
                table: "ContractAddendums",
                newName: "contractId");

            migrationBuilder.RenameColumn(
                name: "ChangeField",
                table: "ContractAddendums",
                newName: "changeField");

            migrationBuilder.RenameColumn(
                name: "AddendumContent",
                table: "ContractAddendums",
                newName: "addendumContent");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ContractAddendums",
                newName: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "workExperiences",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "position",
                table: "workExperiences",
                newName: "Position");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "workExperiences",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "workExperiences",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "workExperiences",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "companyName",
                table: "workExperiences",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "workExperiences",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "terminationType",
                table: "Terminations",
                newName: "TerminationType");

            migrationBuilder.RenameColumn(
                name: "terminationDate",
                table: "Terminations",
                newName: "TerminationDate");

            migrationBuilder.RenameColumn(
                name: "terminationCode",
                table: "Terminations",
                newName: "TerminationCode");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "Terminations",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "Terminations",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "Terminations",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "contractId",
                table: "Terminations",
                newName: "ContractId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Terminations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sys_Dmtts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "positionName",
                table: "Positions",
                newName: "PositionName");

            migrationBuilder.RenameColumn(
                name: "positionCode",
                table: "Positions",
                newName: "PositionCode");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Positions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Positions",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Positions_id",
                table: "Positions",
                newName: "IX_Positions_Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ListOptions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "maxDaysAllowed",
                table: "LeaveTypes",
                newName: "MaxDaysAllowed");

            migrationBuilder.RenameColumn(
                name: "maxCarryForwardDays",
                table: "LeaveTypes",
                newName: "MaxCarryForwardDays");

            migrationBuilder.RenameColumn(
                name: "leaveTypeName",
                table: "LeaveTypes",
                newName: "LeaveTypeName");

            migrationBuilder.RenameColumn(
                name: "isPaid",
                table: "LeaveTypes",
                newName: "IsPaid");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "LeaveTypes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "carryForward",
                table: "LeaveTypes",
                newName: "CarryForward");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "LeaveTypes",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveTypes_id",
                table: "LeaveTypes",
                newName: "IX_LeaveTypes_Id");

            migrationBuilder.RenameColumn(
                name: "totalDays",
                table: "LeaveRequests",
                newName: "TotalDays");

            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "LeaveRequests",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "rejectReason",
                table: "LeaveRequests",
                newName: "RejectReason");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "LeaveRequests",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "LeaveRequests",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "departmentID",
                table: "LeaveRequests",
                newName: "DepartmentID");

            migrationBuilder.RenameColumn(
                name: "cancelReason",
                table: "LeaveRequests",
                newName: "CancelReason");

            migrationBuilder.RenameColumn(
                name: "approvalUser",
                table: "LeaveRequests",
                newName: "ApprovalUser");

            migrationBuilder.RenameColumn(
                name: "approvalStatus",
                table: "LeaveRequests",
                newName: "ApprovalStatus");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "LeaveRequests",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_LeaveRequests_id",
                table: "LeaveRequests",
                newName: "IX_LeaveRequests_Id");

            migrationBuilder.RenameColumn(
                name: "workCode",
                table: "leaveRequest_Details",
                newName: "WorkCode");

            migrationBuilder.RenameColumn(
                name: "totalDays",
                table: "leaveRequest_Details",
                newName: "TotalDays");

            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "leaveRequest_Details",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "leaveRequest_Details",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "leaveType",
                table: "leaveRequest_Details",
                newName: "LeaveType");

            migrationBuilder.RenameColumn(
                name: "leaveRequestId",
                table: "leaveRequest_Details",
                newName: "LeaveRequestId");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "leaveRequest_Details",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "leaveRequest_Details",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "leaveBalanceHistories",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "leaveBalanceHistories",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "daysChanged",
                table: "leaveBalanceHistories",
                newName: "DaysChanged");

            migrationBuilder.RenameColumn(
                name: "changeDate",
                table: "leaveBalanceHistories",
                newName: "ChangeDate");

            migrationBuilder.RenameColumn(
                name: "changeBy",
                table: "leaveBalanceHistories",
                newName: "ChangeBy");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "leaveBalanceHistories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "workingTime",
                table: "EmploymentContracts",
                newName: "WorkingTime");

            migrationBuilder.RenameColumn(
                name: "terms",
                table: "EmploymentContracts",
                newName: "Terms");

            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "EmploymentContracts",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "signedDate",
                table: "EmploymentContracts",
                newName: "SignedDate");

            migrationBuilder.RenameColumn(
                name: "salary",
                table: "EmploymentContracts",
                newName: "Salary");

            migrationBuilder.RenameColumn(
                name: "positionId",
                table: "EmploymentContracts",
                newName: "PositionID");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "EmploymentContracts",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "jobDescription",
                table: "EmploymentContracts",
                newName: "JobDescription");

            migrationBuilder.RenameColumn(
                name: "filePath",
                table: "EmploymentContracts",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "EmploymentContracts",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "EmploymentContracts",
                newName: "EmployeeID");

            migrationBuilder.RenameColumn(
                name: "departmentId",
                table: "EmploymentContracts",
                newName: "DepartmentID");

            migrationBuilder.RenameColumn(
                name: "contractTypeId",
                table: "EmploymentContracts",
                newName: "ContractTypeID");

            migrationBuilder.RenameColumn(
                name: "contractCode",
                table: "EmploymentContracts",
                newName: "ContractCode");

            migrationBuilder.RenameColumn(
                name: "benefits",
                table: "EmploymentContracts",
                newName: "Benefits");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "EmploymentContracts",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_EmploymentContracts_id",
                table: "EmploymentContracts",
                newName: "IX_EmploymentContracts_Id");

            migrationBuilder.RenameColumn(
                name: "taxCode",
                table: "Employees",
                newName: "TaxCode");

            migrationBuilder.RenameColumn(
                name: "positionId",
                table: "Employees",
                newName: "PositionID");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "Employees",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "netSalary",
                table: "Employees",
                newName: "NetSalary");

            migrationBuilder.RenameColumn(
                name: "joinDate",
                table: "Employees",
                newName: "JoinDate");

            migrationBuilder.RenameColumn(
                name: "idCardNumber",
                table: "Employees",
                newName: "IDCardNumber");

            migrationBuilder.RenameColumn(
                name: "idCardIssuePlacec",
                table: "Employees",
                newName: "IDCardIssuePlacec");

            migrationBuilder.RenameColumn(
                name: "idCardIssueDate",
                table: "Employees",
                newName: "IDCardIssueDate");

            migrationBuilder.RenameColumn(
                name: "gender",
                table: "Employees",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "fullName",
                table: "Employees",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "employeeCode",
                table: "Employees",
                newName: "EmployeeCode");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Employees",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "dob",
                table: "Employees",
                newName: "DOB");

            migrationBuilder.RenameColumn(
                name: "departmentId",
                table: "Employees",
                newName: "DepartmentID");

            migrationBuilder.RenameColumn(
                name: "contractTypeId",
                table: "Employees",
                newName: "ContractTypeID");

            migrationBuilder.RenameColumn(
                name: "bankName",
                table: "Employees",
                newName: "BankName");

            migrationBuilder.RenameColumn(
                name: "bankBranch",
                table: "Employees",
                newName: "BankBranch");

            migrationBuilder.RenameColumn(
                name: "bankAccountNumber",
                table: "Employees",
                newName: "BankAccountNumber");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Employees",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Employees",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_id",
                table: "Employees",
                newName: "IX_Employees_Id");

            migrationBuilder.RenameColumn(
                name: "usedDays",
                table: "employeeLeaveBalances",
                newName: "UsedDays");

            migrationBuilder.RenameColumn(
                name: "totalDays",
                table: "employeeLeaveBalances",
                newName: "TotalDays");

            migrationBuilder.RenameColumn(
                name: "remainingDays",
                table: "employeeLeaveBalances",
                newName: "RemainingDays");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "employeeLeaveBalances",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "employeeLeaveBalances",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "usedDaysMonth",
                table: "employeeLeaveBalances",
                newName: "UsedDays_month");

            migrationBuilder.RenameColumn(
                name: "remainingDaysMonth",
                table: "employeeLeaveBalances",
                newName: "RemainingDays_month");

            migrationBuilder.RenameColumn(
                name: "maxDayMonth",
                table: "employeeLeaveBalances",
                newName: "Maxday_month");

            migrationBuilder.RenameColumn(
                name: "notes",
                table: "EmployeeDocuments",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "filePath",
                table: "EmployeeDocuments",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "EmployeeDocuments",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "documentType",
                table: "EmployeeDocuments",
                newName: "DocumentType");

            migrationBuilder.RenameColumn(
                name: "documentName",
                table: "EmployeeDocuments",
                newName: "DocumentName");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "EmployeeDocuments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "relationship",
                table: "emergencyContacts",
                newName: "Relationship");

            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "emergencyContacts",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "fullName",
                table: "emergencyContacts",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "emergencyContacts",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "emergencyContacts",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "emergencyContacts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "Educations",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "school",
                table: "Educations",
                newName: "School");

            migrationBuilder.RenameColumn(
                name: "major",
                table: "Educations",
                newName: "Major");

            migrationBuilder.RenameColumn(
                name: "gpa",
                table: "Educations",
                newName: "GPA");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "Educations",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "employeeId",
                table: "Educations",
                newName: "EmployeeId");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Educations",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "degree",
                table: "Educations",
                newName: "Degree");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Educations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "managerID",
                table: "Departments",
                newName: "ManagerID");

            migrationBuilder.RenameColumn(
                name: "departmentName",
                table: "Departments",
                newName: "DepartmentName");

            migrationBuilder.RenameColumn(
                name: "departmentCode",
                table: "Departments",
                newName: "DepartmentCode");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Departments",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_id",
                table: "Departments",
                newName: "IX_Departments_Id");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "ContractTypes",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "contractTypeName",
                table: "ContractTypes",
                newName: "ContractTypeName");

            migrationBuilder.RenameColumn(
                name: "contractTypeCode",
                table: "ContractTypes",
                newName: "ContractTypeCode");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ContractTypes",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ContractTypes_id",
                table: "ContractTypes",
                newName: "IX_ContractTypes_Id");

            migrationBuilder.RenameColumn(
                name: "oldValue",
                table: "ContractHistories",
                newName: "OldValue");

            migrationBuilder.RenameColumn(
                name: "newValue",
                table: "ContractHistories",
                newName: "NewValue");

            migrationBuilder.RenameColumn(
                name: "desciption",
                table: "ContractHistories",
                newName: "Desciption");

            migrationBuilder.RenameColumn(
                name: "contractId",
                table: "ContractHistories",
                newName: "ContractId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ContractHistories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "ContractAddendums",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "salary",
                table: "ContractAddendums",
                newName: "Salary");

            migrationBuilder.RenameColumn(
                name: "positionID",
                table: "ContractAddendums",
                newName: "PositionID");

            migrationBuilder.RenameColumn(
                name: "endDate",
                table: "ContractAddendums",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "effectiveDate",
                table: "ContractAddendums",
                newName: "EffectiveDate");

            migrationBuilder.RenameColumn(
                name: "departmentID",
                table: "ContractAddendums",
                newName: "DepartmentID");

            migrationBuilder.RenameColumn(
                name: "contractId",
                table: "ContractAddendums",
                newName: "ContractId");

            migrationBuilder.RenameColumn(
                name: "changeField",
                table: "ContractAddendums",
                newName: "ChangeField");

            migrationBuilder.RenameColumn(
                name: "addendumContent",
                table: "ContractAddendums",
                newName: "AddendumContent");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ContractAddendums",
                newName: "Id");
        }
    }
}
