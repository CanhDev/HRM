import { BaseEntity, IActionDto } from "src/app/core/models/base";

export interface EmployeeDocument extends BaseEntity {
    employeeId: number;
    folderId: number;
    documentName: string;
    documentType?: string;
    filePath?: string;
    notes?: string;
    tags?: string;
    expiryDate?: Date;
    isCompany?: boolean;
    createdDate?: Date;
    documentUrl?: string;
}

export interface EmployeeDocumentFolder extends BaseEntity {
    employeeId: number;
    folderName: string;
    description?: string;
    documents?: EmployeeDocument[]; // optional để tránh lỗi undefined nếu không load
}

// DTO objects for API transfers
export interface EmployeeDocumentDTO extends IActionDto {
    employeeId: number;
    folderId: number;
    documentName: string;
    documentType?: string;
    filePath?: string;
    notes?: string;
    tags?: string;
    expiryDate?: Date | null;
    isCompany?: boolean;
    status?: number;
    documentUrl?: string;
    docfile?: File; // Sử dụng cho upload file
}

export interface EmployeeDocumentFolderDTO  {
    id: number;
    employeeId: number;
    folderName: string;
    description?: string;
    status?: number;
}

// Combined model for creating folder with documents
export interface EmployeeDocFolderWithDocs {
    folder: EmployeeDocumentFolderDTO;
    documents: EmployeeDocumentDTO[];
}

// Tag model for UI display
export interface DocumentTag {
    name: string;
    color: string;
}