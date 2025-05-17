import { BaseEntity } from "src/app/core/models/base";
export interface Position extends BaseEntity{
    positionCode: string;
    positionName: string;
    description: string;
}