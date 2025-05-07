import { BaseEntity, IActionDto } from "src/app/core/models/base";

  export interface Employee extends BaseEntity {
    employeeCode: string;
    fullName?: string;
    dob: string;               // ISO date string
    gender?: string;
    idCardNumber?: string;
    idCardIssueDate?: string;  // ISO date string
    idCardIssuePlace?: string;
    taxCode?: string;
    bankAccountNumber?: string;
    bankName?: string;
    bankBranch?: string;
    joinDate?: string;         // ISO date string
    phone?: string;
    email?: string;
    address?: string;
    imageUrl?: string;
    netSalary: number;
    departmentId?: number;
    positionId?: number;
    contractTypeId?: number;
    actionType?: string;
  }

  export interface EmployeeDTO {
    employeeCode: string;
    fullName: string;
    dob?: Date;
    gender?: string;
    idCardNumber?: string;
    idCardIssueDate?: Date;
    idCardIssuePlace?: string;
    taxCode?: string;
    bankAccountNumber?: string;
    bankName?: string;
    bankBranch?: string;
    joinDate?: Date;
    phone?: string;
    email?: string;
    address?: string;
    netSalary?: number;
    departmentId?: number;
    positionId?: number;
    contractTypeId?: number;
    imageUrl?: string;
    imageFile?: File;
    status: number;
  }

  export interface EmergencyContactDTO extends IActionDto {
    employeeId?: number;
    relationship?: string;
    fullName?: string;
    phoneNumber?: string;
    address?: string;
    status: number;
  }
  export interface EducationDTO extends IActionDto {
    employeeId?: number;
    degree?: string;
    major?: string;
    school?: string;
    startDate?: Date;
    endDate?: Date;
    gpa?: number;
    description?: string;
    status: number;
  }
  export interface WorkExperienceDTO extends IActionDto {
    employeeId?: number;
    companyName?: string;
    position?: string;
    startDate?: Date;
    endDate?: Date;
    description?: string;
    status: number;
  }
  export interface EmployeeDataset {
    employeeDTO: EmployeeDTO;
    emergencyContactList?: EmergencyContactDTO[];
    educationList?: EducationDTO[];
    workExperienceList?: WorkExperienceDTO[];
  }
  export interface EmployeeCreateRequest {
    employeeDTO: EmployeeDTO;
    emergencyContactsJson: string;
    educationListJson: string;
    workExperienceListJson: string;
    imageFile?: File;
  }

  //res
  export interface EmployeeResponse {
    id: number;
    employeeCode: string;
    fullName: string;
    dob: Date;
    gender: string;
    idCardNumber: string;
    idCardIssueDate: Date;
    idCardIssuePlace: string;
    taxCode: string;
    bankAccountNumber: string;
    bankName: string;
    bankBranch: string;
    joinDate: Date;
    phone: string;
    email: string;
    address: string;
    netSalary: number;
    departmentId: number;
    positionId: number;
    contractTypeId: number;
    imageUrl?: string;
    createdAt?: Date;
    updateAt?: Date;
    createBy?: string;
    updateBy?: string;
    status?: number;
  }
  export interface EmergencyContactResponse {
    id: number;
    employeeId: number;
    relationship: string;
    fullName: string;
    phoneNumber: string;
    address: string;
    status?: number;
  }
  
  export interface EducationResponse {
    id: number;
    employeeId: number;
    degree: string;
    major: string;
    school: string;
    startDate: Date;
    endDate: Date;
    gpa: number;
    description: string;
    status?: number;
  }
  
  export interface WorkExperienceResponse {
    id: number;
    employeeId: number;
    companyName: string;
    position: string;
    startDate?: Date;
    endDate?: Date;
    description: string;
    status?: number;
  }
  
  export interface EmployeeDataResponse {
    employeeRes: EmployeeResponse;
    emergencyContactList?: EmergencyContactResponse[];
    educationList?: EducationResponse[];
    workExperienceList?: WorkExperienceResponse[];
  }
  