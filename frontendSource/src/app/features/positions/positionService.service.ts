import { Injectable } from '@angular/core';
import { Position } from './entity';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ApiResponseBasic, SearchRequest } from 'src/app/core/models/base';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PositionServiceService {

constructor(private http: HttpClient) { }
  private apiUrl = `${environment.apiBaseUrl}Position`;
  getData(searchRequest: SearchRequest<Position>) : Observable<ApiResponseBasic>{
    return this.http.post<ApiResponseBasic>(`${this.apiUrl}/getData`, searchRequest);
  }
  getById(id : number) : Observable<ApiResponseBasic>{
    return this.http.get<ApiResponseBasic>(`${this.apiUrl}/${id}`);
  }
  create(data : Position) : Observable<ApiResponseBasic>{
    return this.http.post<ApiResponseBasic>(`${this.apiUrl}`, data);
  }
  update(id : number, data : Position) : Observable<ApiResponseBasic>{
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
