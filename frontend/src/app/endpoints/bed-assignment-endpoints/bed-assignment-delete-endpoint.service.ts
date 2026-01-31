import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BedAssignDeleteService {
  private apiUrl = 'http://localhost:7000/api/BedUnassignEndpoint';

  constructor(private http: HttpClient) {}

  unassign(assignmentId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${assignmentId}`);
  }
}
