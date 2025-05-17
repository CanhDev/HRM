import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiResponseBasic, SearchRequest } from 'src/app/core/models/base';

import { LeaveRequest, Dataset_Leave, DataResponse_Leave, LeaveBalanceResSub, LeaveRequestDetailsRes, LeaveRequestDetaislDto, LeaveRequestDto, LeaveRequestRes } from './entity';

@Injectable({
  providedIn: 'root'
})
export class LeaveRequestService {
constructor(private http: HttpClient) { }
private apiUrl = `${environment.apiBaseUrl}Leave`;

    getData(searchRequest: SearchRequest<LeaveRequest>) : Observable<ApiResponseBasic>{
      return this.http.post<ApiResponseBasic>(`${this.apiUrl}/getData`, searchRequest);
    }
    getNew() : Observable<ApiResponseBasic>{
      return this.http.get<ApiResponseBasic>(`${this.apiUrl}/GetNew/22`);
    }
    
    getById(id : number) : Observable<ApiResponseBasic>{
      return this.http.get<ApiResponseBasic>(`${this.apiUrl}/${id}`);
    }
    create(data : Dataset_Leave) : Observable<ApiResponseBasic>{
      return this.http.post<ApiResponseBasic>(`${this.apiUrl}`, data);
    }
    update(id : number, data : Dataset_Leave) : Observable<ApiResponseBasic>{
      return this.http.put<ApiResponseBasic>(`${this.apiUrl}/${id}`, data);
    }
    Approve(id : number, approverId: number) : Observable<ApiResponseBasic>{
      const url =  `${this.apiUrl}/Approve/${id}?approverId=${approverId}`;
      return this.http.post<ApiResponseBasic>(url, {});
    }
    Reject(id : number, approverId: number, reason: string) : Observable<ApiResponseBasic>{
      const url =  `${this.apiUrl}/Reject/${id}?approverId=${approverId}&reason=${encodeURIComponent(reason)}`;
      return this.http.post<ApiResponseBasic>(url, {});
    }
    delete(id : number) : Observable<ApiResponseBasic>{
      return this.http.delete<ApiResponseBasic>(`${this.apiUrl}/${id}`);
    }
    deleteRange(ids: number[]): Observable<any> {
      return this.http.delete(`${this.apiUrl}/DeleteRange`, {
        body: ids  
      });
    }

}
