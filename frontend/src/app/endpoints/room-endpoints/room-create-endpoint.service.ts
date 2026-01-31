import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface RoomCreateRequest {
  roomNumber: string;
  floor: number;
  description: string;
}

@Injectable({
  providedIn: 'root'
})
export class RoomCreateEndpointService {

  private apiUrl = 'http://localhost:7000/api/RoomCreateEndpoint/create';

  constructor(private http: HttpClient) {}

  createRoom(request: RoomCreateRequest): Observable<any> {
    return this.http.post<any>(this.apiUrl, request);
  }
}
