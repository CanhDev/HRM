import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Employee } from 'src/app/features/employeeDoc/models/entity';
import { ApiResponseBasic, PagedResult, SearchRequest } from 'src/app/core/models/base';

@Injectable({
  providedIn: 'root'
})
export class LookupService {
  private baseUrl = `${environment.apiBaseUrl}LookUp`;
  constructor(private http: HttpClient) {}

  getContractTypes(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/contract-types`);
  }

  getLeaveTypes(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/leave-types`);
  }

  getEmployees(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/employees`);
  }

  getDepartments(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/departments`);
  }

  getPositions(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/positions`);
  }

  getSysListOptions(form: string, form_type: string): Observable<any[]> {
    const params = new HttpParams()
      .set('form', form)
      .set('form_type', form_type);
    return this.http.get<any[]>(`${this.baseUrl}/sys_listoptions`, { params });
  }

  getSysDmtt(ma_ct: string): Observable<any[]> {
    const params = new HttpParams().set('ma_ct', ma_ct);
    return this.http.get<any[]>(`${this.baseUrl}/sys_dmtt`, { params });
  }
/**
 * Chuyển đổi chuỗi datetime thành định dạng yyyy-MM-dd
 * @param dateString Chuỗi datetime (ví dụ: "2025-05-02T00:00:00")
 * @returns Chuỗi ngày tháng định dạng yyyy-MM-dd hoặc chuỗi rỗng nếu đầu vào không hợp lệ
 */
formatDate(dateString: string): string {
  if (!dateString) {
    return '';
  }
  
  try {
    // Tạo đối tượng Date từ chuỗi datetime
    const date = new Date(dateString);
    
    // Kiểm tra xem date có hợp lệ không
    if (isNaN(date.getTime())) {
      return '';
    }
    
    // Lấy năm, tháng, ngày
    const year = date.getFullYear();
    // Tháng trong JavaScript bắt đầu từ 0, nên cần +1
    // padStart đảm bảo luôn có 2 chữ số (thêm số 0 ở đầu nếu cần)
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    
    // Trả về định dạng yyyy-MM-dd
    return `${year}-${month}-${day}`;
  } catch (error) {
    console.error('Lỗi khi định dạng ngày tháng:', error);
    return '';
  }
}
}
