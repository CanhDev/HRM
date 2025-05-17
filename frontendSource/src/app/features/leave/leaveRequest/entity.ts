import { BaseEntity, BaseVoucherEntity, IActionDto } from "src/app/core/models/base";

export interface Dataset_Leave{
    ph: LeaveRequestDto;
    ct: LeaveRequestDetaislDto[];
}
export interface DataResponse_Leave{
    ph: LeaveRequestRes;
    ct: LeaveRequestDetailsRes[];
    ctBalance: LeaveBalanceResSub[];
}

///

export interface LeaveRequestRes extends BaseVoucherEntity{
    employeeId : number;
    startDate : Date;
    endDate: Date;
    totalDays: number;
}
export interface LeaveRequestDetailsRes{
    leaveRequestId : number;
    workCode : number;
    startDate : Date;
    endDate: Date;
    totalDays : number;
    leaveType: string;
    reason: string;
    status: number;
    id: number;
}
export interface LeaveBalanceResSub{
    usedDaysMonth : number;
    remainingDaysMonth : number;
    maxDayMonth: number;
}

export interface LeaveRequestDto{
    voucherCode : string;
    employeeId: number;
    totalDays: number;
    approvalStatus: number;
    createDate: Date;
    status: number;
    departmentId : number;
    detailNote: string;
}
export interface LeaveRequestDetaislDto extends IActionDto{
    status : number;
    reason : string;
    leaveType : string;
    totalDays: number;
    endDate: Date;
    startDate: Date;
    workCode: number;
    leaveRequestId: number;
}
export interface LeaveRequest extends BaseVoucherEntity{
    employeeId : number;
    createDate: Date;
    totalDays: number;
  }