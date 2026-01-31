import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BedAssignGetByRoomService {
  private apiUrl = 'http://localhost:7000/api/BedAssignmentGetByRoomIdEndpoint';

  constructor(private http: HttpClient) {}

  getByRoom(roomId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${roomId}`);
  }
}
