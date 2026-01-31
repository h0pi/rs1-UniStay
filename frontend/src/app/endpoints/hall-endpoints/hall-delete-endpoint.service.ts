import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HallDeleteEndpointService {
  private apiUrl = 'http://localhost:7000/api/HallDeleteEndpoint';

  constructor(private http: HttpClient) {}

  deleteHall(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}