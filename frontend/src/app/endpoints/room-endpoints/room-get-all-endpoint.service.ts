import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoomGetAllEndpointService {
  private apiUrl = 'http://localhost:7000/api/RoomGetAllEndpoint/filter';

  constructor(private http: HttpClient) {}

  getAllRooms(q: string = "", pageNumber = 1, pageSize = 10) {
    return this.http.get<any>(`${this.apiUrl}?q=${q}&pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }
}
