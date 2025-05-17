import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { 
  EmployeeDocument, 
  EmployeeDocumentFolder,
  EmployeeDocumentDTO,
  EmployeeDocumentFolderDTO,
  EmployeeDocFolderWithDocs,
  DocumentTag
} from './entity';
import { ApiResponseBasic } from 'src/app/core/models/base';

@Injectable({
  providedIn: 'root'
})
export class EmployeeDocumentService {
  private apiUrl = `${environment.apiBaseUrl}EmployeeDocument`;

  constructor(private http: HttpClient) { }

  // Folder operations
  getFoldersByEmployeeId(employeeId: number): Observable<ApiResponseBasic> {
    return this.http.get<ApiResponseBasic>(`${this.apiUrl}/folders/${employeeId}`);
  }

  getFolderById(folderId: number): Observable<ApiResponseBasic> {
    return this.http.get<ApiResponseBasic>(`${this.apiUrl}/folder/${folderId}`);
  }

  createFolder(folder: EmployeeDocumentFolderDTO): Observable<ApiResponseBasic> {
    return this.http.post<ApiResponseBasic>(`${this.apiUrl}/folder`, folder);
  }

  updateFolder(folder: EmployeeDocumentFolderDTO): Observable<ApiResponseBasic> {
    return this.http.put<ApiResponseBasic>(`${this.apiUrl}/folder/${folder.id}`, folder);
  }

  deleteFolder(folderId: number): Observable<ApiResponseBasic> {
    return this.http.delete<ApiResponseBasic>(`${this.apiUrl}/folder/${folderId}`);
  }

  // Document operations
  getDocumentsByFolderId(folderId: number): Observable<ApiResponseBasic> {
    return this.http.get<ApiResponseBasic>(`${this.apiUrl}/documents/${folderId}`);
  }

  getDocumentById(documentId: number): Observable<ApiResponseBasic> {
    return this.http.get<ApiResponseBasic>(`${this.apiUrl}/document/${documentId}`);
  }

  createDocument(document: EmployeeDocumentDTO, file?: File): Observable<ApiResponseBasic> {
    const formData = new FormData();
    
    // Add document data to FormData
    Object.keys(document as Record<string, any>).forEach(key => {
      const value = (document as Record<string, any>)[key];
      if (value !== null && value !== undefined) {
        if (key === 'expiryDate' && value) {
          formData.append(key, (value as Date).toISOString());
        } else {
          formData.append(key, value);
        }
      }
    });
    
    // Add file if exists
    if (file) {
      formData.append('docfile', file, file.name);
    }
    
    return this.http.post<ApiResponseBasic>(`${this.apiUrl}/document`, formData);
}

  updateDocument(document: EmployeeDocumentDTO, file?: File): Observable<ApiResponseBasic> {
    const formData = new FormData();
    
    // Add document data to FormData
    Object.keys(document as Record<string, any>).forEach(key => {
      const value = (document as Record<string, any>)[key];
      if (value !== null && value !== undefined) {
        if (key === 'expiryDate' && value) {
        const date = new Date(value);
        if (!isNaN(date.getTime())) {
          formData.append(key, date.toISOString());
        }
      }
    else {
          formData.append(key, value);
        }
      }
    });
    
    // Add file if exists
    if (file) {
      formData.append('docfile', file, file.name);
    }
    
    return this.http.put<ApiResponseBasic>(`${this.apiUrl}/document/${document.id}`, formData);
}

  deleteDocument(documentId: number): Observable<ApiResponseBasic> {
    return this.http.delete<ApiResponseBasic>(`${this.apiUrl}/document/${documentId}`);
  }

  // Combined operations
  createFolderWithDocuments(folderWithDocs: EmployeeDocFolderWithDocs, files: File[]): Observable<ApiResponseBasic> {
    const formData = new FormData();
    
    // Add folder data
    formData.append('folder.id', folderWithDocs.folder.id.toString());
    formData.append('folder.employeeId', folderWithDocs.folder.employeeId.toString());
    formData.append('folder.folderName', folderWithDocs.folder.folderName);
    if (folderWithDocs.folder.description) {
      formData.append('folder.description', folderWithDocs.folder.description);
    }
    
    // Add documents data
    if (folderWithDocs.documents && folderWithDocs.documents.length > 0) {
  folderWithDocs.documents.forEach((doc, index) => {
    Object.keys(doc as Record<string, any>).forEach(key => {
      const value = (doc as Record<string, any>)[key];
      if (value !== null && value !== undefined) {
        if (key === 'expiryDate' && value) {
          const date = new Date(value);
          if (!isNaN(date.getTime())) {
            formData.append(`documents[${index}].${key}`, date.toISOString());
          }
        } else {
          formData.append(`documents[${index}].${key}`, value);
        }
      }
    });

    if (files[index]) {
      formData.append(`documents[${index}].docfile`, files[index], files[index].name);
    }
  });
}

    
    return this.http.post<ApiResponseBasic>(`${this.apiUrl}/folder-with-documents`, formData);
}

  // File operations
  downloadDocument(documentId: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/download/${documentId}`, {
      responseType: 'blob'
    });
  }

  getDocumentUrl(documentId: number): string {
    return `${this.apiUrl}/download/${documentId}`;
  }

  // Tag operations
  getAllTags(): Observable<ApiResponseBasic> {
    return this.http.get<ApiResponseBasic>(`${this.apiUrl}/tags`);
  }

  parseTags(tagsString: string): DocumentTag[] {
    if (!tagsString) return [];
    
    // Split by comma and create tag objects
    return tagsString.split(',')
      .map(tag => tag.trim())
      .filter(tag => tag.length > 0)
      .map(tag => ({ 
        name: tag, 
        color: this.getColorForTag(tag)
      }));
  }

  // Helper method to generate consistent colors for tags
  private getColorForTag(tag: string): string {
    // Simple hash function for string
    let hash = 0;
    for (let i = 0; i < tag.length; i++) {
      hash = ((hash << 5) - hash) + tag.charCodeAt(i);
      hash |= 0; // Convert to 32bit integer
    }
    
    // Array of predefined colors
    const colors = ['#4CAF50', '#2196F3', '#FFC107', '#E91E63', '#9C27B0', '#FF5722', '#607D8B'];
    const index = Math.abs(hash) % colors.length;
    
    return colors[index];
  }
}