import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BedAssignGetAllService {
  private apiUrl = 'http://localhost:7000/api/BedAssignmentGetAllEndpoint/all';

  constructor(private http: HttpClient) {}

  getAll(params: any): Observable<any> {
    return this.http.get(this.apiUrl, { params });
  }
}
