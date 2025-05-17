import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiResponseBasic, SearchRequest } from 'src/app/core/models/base';
import { ContractAddendum, ContractAddendumDTO, ContractHistory,
   ContractHistoryDTO, EmploymentContract, EmploymentContractDTO, ContractDataset, ContractDatares, RejectModel } from './entity';

@Injectable({
  providedIn: 'root'
})
export class ContractsService {

constructor(private http: HttpClient) { }
  private apiUrl = `${environment.apiBaseUrl}Contract`;

    getData(searchRequest: SearchRequest<EmploymentContract>) : Observable<ApiResponseBasic>{
      return this.http.post<ApiResponseBasic>(`${this.apiUrl}/getData`, searchRequest);
    }
    getNew() : Observable<ApiResponseBasic>{
      return this.http.get<ApiResponseBasic>(`${this.apiUrl}/GetNew`);
    }
    checkStatus(id: number) : Observable<ApiResponseBasic>{
      return this.http.get<ApiResponseBasic>(`${this.apiUrl}/CheckStatus/${id}`);
    }
    getById(id : number) : Observable<ApiResponseBasic>{
      return this.http.get<ApiResponseBasic>(`${this.apiUrl}/${id}`);
    }
    create(data : ContractDataset) : Observable<ApiResponseBasic>{
      return this.http.post<ApiResponseBasic>(`${this.apiUrl}`, data);
    }
    update(id : number, data : ContractDataset) : Observable<ApiResponseBasic>{
      return this.http.put<ApiResponseBasic>(`${this.apiUrl}/${id}`, data);
    }
    Reject(id : number, data : RejectModel) : Observable<ApiResponseBasic>{
      return this.http.put<ApiResponseBasic>(`${this.apiUrl}/Reject/${id}`, data);
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
