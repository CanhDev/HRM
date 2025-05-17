import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiResponseBasic, SearchRequest } from 'src/app/core/models/base';

import { LeaveType } from './entity';


@Injectable({
  providedIn: 'root'
})
export class LeaveTypeService {

constructor(private http: HttpClient) { }
private apiUrl = `${environment.apiBaseUrl}LeaveType`;
  getData(searchRequest: SearchRequest<LeaveType>) : Observable<ApiResponseBasic>{
    return this.http.post<ApiResponseBasic>(`${this.apiUrl}/getData`, searchRequest);
  }
  getById(id : number) : Observable<ApiResponseBasic>{
    return this.http.get<ApiResponseBasic>(`${this.apiUrl}/${id}`);
  }
  create(data : LeaveType) : Observable<ApiResponseBasic>{
    return this.http.post<ApiResponseBasic>(`${this.apiUrl}`, data);
  }
  update(id : number, data : LeaveType) : Observable<ApiResponseBasic>{
    return this.http.put<ApiResponseBasic>(`${this.apiUrl}/${id}`, data);
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
