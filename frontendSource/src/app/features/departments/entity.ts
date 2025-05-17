import { BaseEntity, IActionDto } from "src/app/core/models/base";
export interface Department extends BaseEntity {
    departmentCode: string;
    departmentName: string;
    managerID? : number;
}