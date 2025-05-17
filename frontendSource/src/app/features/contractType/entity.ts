import { BaseEntity } from "src/app/core/models/base";
export interface ContractType extends BaseEntity{
    contractTypeCode: string;
    contractTypeName: string;
    description: string;
}