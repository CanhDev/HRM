import { BaseEntity } from "src/app/core/models/base";
export interface EmployeeLeaveBalance extends BaseEntity{
    employeeId: number;
    totalDays : number;
    usedDays : number;
    remainingDays : number;
    usedDaysMonth : number;
    remainingDaysMonth: number;
    maxDayMonth: number;
}