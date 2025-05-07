import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Employee, EmployeeCreateRequest, EmployeeDataResponse, EmployeeResponse } from '../../models/entity';
import { ApiResponse, ApiResponseBasic, PagedResult, SearchRequest } from 'src/app/core/models/base';
@Injectable({
  providedIn: 'root'
})
export class EmployeeServiceService {
  private apiUrl = `${environment.apiBaseUrl}Employee`;

  constructor(private http: HttpClient) { }

  getEmployees(searchRequest: SearchRequest<Employee>): Observable<ApiResponseBasic> {
    console.log('Calling API with URL:', this.apiUrl, 'and body:', searchRequest);
    return this.http.post<ApiResponseBasic>(this.apiUrl + '/getData', searchRequest);
  }

  // Get employee by ID
  getById(id: number): Observable<ApiResponseBasic> {
    return this.http.get<ApiResponseBasic>(`${this.apiUrl}/${id}`);
  }

  create(employeeData: EmployeeCreateRequest): Observable<ApiResponse<EmployeeResponse>> {
    const formData = this.prepareFormData(employeeData);
    
    // Thêm log để debug
    console.log('Sending form data to:', this.apiUrl);
    formData.forEach((value, key) => {
      if (typeof value === 'string' && value.length < 100) {
        console.log(`${key}: ${value}`);
      } else {
        console.log(`${key}: [Data omitted for brevity]`);
      }
    });
    
    return this.http.post<ApiResponse<EmployeeResponse>>(this.apiUrl, formData);
  }

  

  // Update employee
  update(id: number, employeeData: EmployeeCreateRequest): Observable<ApiResponse<EmployeeResponse>> {
    const formData = this.prepareFormData(employeeData);
    return this.http.put<ApiResponse<EmployeeResponse>>(`${this.apiUrl}/${id}`, formData);
  }

  // Delete employee
  delete(id: number): Observable<ApiResponse<EmployeeResponse>> {
    return this.http.delete<ApiResponse<EmployeeResponse>>(`${this.apiUrl}/${id}`);
  }

  deleteEmployee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  deleteRange(ids: number[]): Observable<any> {
    const params = new HttpParams({ fromObject: { ids: ids.map(id => id.toString()) } });
    return this.http.delete(`${this.apiUrl}/DeleteRange`, { params });
  }
  deleteAll(): Observable<any> {
    return this.http.delete(`${this.apiUrl}/DeleteAll`);
  }
  // Helper to prepare FormData for API requests
  private prepareFormData(data: EmployeeCreateRequest): FormData {
    const formData = new FormData();
    
    // Thay vì gửi employeeDTO như một JSON string
    // Thêm từng trường vào form data riêng biệt
    if (data.employeeDTO) {
      Object.entries(data.employeeDTO).forEach(([key, value]) => {
        // Bỏ qua các giá trị null/undefined
        if (value !== null && value !== undefined) {
          if (key === 'imageFile' && value instanceof File) {
            // Xử lý file trong employeeDTO (nếu có)
          } else {
            // Gửi dưới dạng EmployeeDTO.key
            formData.append(`EmployeeDTO.${key}`, value.toString());
          }
        }
      });
    }
    
    // Thêm các trường JSON khác
    if (data.emergencyContactsJson) {
      formData.append('emergencyContactsJson', data.emergencyContactsJson);
    }
    
    if (data.educationListJson) {
      formData.append('educationListJson', data.educationListJson);
    }
    
    if (data.workExperienceListJson) {
      formData.append('workExperienceListJson', data.workExperienceListJson);
    }
    
    // Thêm file ảnh nếu có
    if (data.imageFile) {
      formData.append('ImageFile', data.imageFile, data.imageFile.name);
    }
    
    // In ra để kiểm tra
    console.log('FormData prepared:');
    formData.forEach((value, key) => {
      console.log(`${key}:`, value);
    });
    
    return formData;
  }
}
