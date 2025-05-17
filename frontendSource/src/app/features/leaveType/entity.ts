import { BaseEntity } from "src/app/core/models/base";

export interface LeaveType extends BaseEntity{
    leaveTypeCode: string;
    leaveTypeName : string;
    description: string;
    isPaid: number;
    maxDaysAllowed: number;
}