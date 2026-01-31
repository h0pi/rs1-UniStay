import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BedAssignCreateService {
  private apiUrl = 'http://localhost:7000/api/BedAssignEndpoint/assign';

  constructor(private http: HttpClient) {}

  assign(request: any): Observable<any> {
    return this.http.post(this.apiUrl, request);
  }
}
