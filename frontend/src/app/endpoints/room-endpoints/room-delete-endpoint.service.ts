import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoomDeleteEndpointService {
  private apiUrl = 'http://localhost:7000/api/RoomDeleteEndpoint';

  constructor(private http: HttpClient) {}

  deleteRoom(id: number): Observable<any> {
    return this.http.delete<{message: string}>(`${this.apiUrl}/${id}`);
  }
}
