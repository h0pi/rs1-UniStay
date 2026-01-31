import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BedAssignGetByStudentService {
  private apiUrl = 'http://localhost:7000/api/BedAssignmentGetByStudent';

  constructor(private http: HttpClient) {}

  getByStudent(studentId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${studentId}`);
  }
}
