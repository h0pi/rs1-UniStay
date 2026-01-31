import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoomGetByIdEndpointService {
  private apiUrl = 'http://localhost:7000/api/RoomGetByIdEndpoint';

  constructor(private http: HttpClient) {}

  getRoomById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}?request=${id}`);
  }
}
