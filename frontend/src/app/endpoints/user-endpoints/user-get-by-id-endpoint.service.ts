import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserGetByIdResponse {
  userID: number;
  email: string;
  firstName: string;
  lastName: string;
  username: string;
  phone: string;
  dateOfBirth: string;
  profileImage?: string;
  roleName?: string;
  createdAt: string;
  updatedAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserGetByIdEndpointService {
  private apiUrl = 'http://localhost:7000/api/UserGetByIdEndpoint';

  constructor(private http: HttpClient) {}

  getById(id: number): Observable<UserGetByIdResponse> {
    return this.http.get<UserGetByIdResponse>(`${this.apiUrl}/${id}`);
  }
}
