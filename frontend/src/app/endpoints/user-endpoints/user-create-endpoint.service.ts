import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserCreateRequest {
  email: string;
  firstName: string;
  lastName: string;
  phone: string;
  dateOfBirth: string;
  password: string;
  username?: string;
  profileImage?: string;
  roleID?: number | null;
}

export interface UserCreateResponse {
  id: number;
}

@Injectable({
  providedIn: 'root'
})
export class UserCreateEndpointService {
  private apiUrl = 'http://localhost:7000/api/UserCreateEndpoint/create';

  constructor(private http: HttpClient) {}

  createUser(request: {
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    email: string;
    password: string;
    phone: string;
    username: string;
    profileImage: string;
    roleID: number
  }): Observable<UserCreateResponse> {
    return this.http.post<UserCreateResponse>(`${this.apiUrl}`, request);
  }
}
