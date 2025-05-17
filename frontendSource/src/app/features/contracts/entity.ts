import { BaseEntity, IActionDto } from "src/app/core/models/base";

export interface ContractDataset{
    contractDTO : EmploymentContractDTO;
    contractAddendumDTOs : ContractAddendumDTO[];
}

export interface ContractDatares{
    contractRes : EmploymentContract;
    contractAddendumRes : ContractAddendum;
    contractHistoryRes : ContractHistory;
}
export interface RejectModel{
    contractId : number;
    rejectReason : string;
}
//

export interface EmploymentContract extends BaseEntity{
    employeeId: number;
    departmentId: number;
    positionId: number;
    contractCode: string;
    contractTypeId: number;
    salary: number;
    notes: string;
    startDate: Date;
    endDate: Date;
    signedDate: Date;
    workingTime: string;
    jobDescription: string;
    benefits: string;
    filePath: string;
    terms: string;
}
export interface EmploymentContractDTO {
    employeeId: number;
    departmentId: number;
    positionId: number;
    contractCode: string;
    contractTypeId: number;
    salary: number;
    notes: string;
    startDate: Date;
    endDate: Date;
    signedDate: Date;
    workingTime: string;
    jobDescription: string;
    benefits: string;
    filePath: string;
    terms: string;
    status :number;
}
export interface ContractAddendum extends BaseEntity{
    contractId: number;
    addendumContent: string;
    effectiveDate: Date;
    changeField: string;
    title: string;
}
export interface ContractAddendumDTO extends IActionDto{
    contractId: number;
    addendumContent: string;
    effectiveDate: Date;
    changeField: string;
    title: string;
    salary: number;
    endDate: Date;
    positionId: number;
    departmentId: number;
}
export interface ContractHistory extends BaseEntity{
    contractId: number;
    newValue: string;
    oldValue: string;
}
export interface ContractHistoryDTO {
    contractId: number;
    newValue: string;
    oldValue: string;
}