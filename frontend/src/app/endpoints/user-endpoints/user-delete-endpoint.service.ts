import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserDeleteEndpointService {
  private apiUrl = 'http://localhost:7000/api/UserDeleteEndpoint';

  constructor(private http: HttpClient) {}

  deleteUser(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${id}`);
  }
}
