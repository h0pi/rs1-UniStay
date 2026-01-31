import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoomUpdateEndpointService {
  private apiUrl = 'http://localhost:7000/api/RoomUpdateEndpoint';

  constructor(private http: HttpClient) {}

  updateRoom(request: any): Observable<any> {
    return this.http.put<any>(this.apiUrl, request);
  }
}
