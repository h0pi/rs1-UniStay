import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserUpdateRequest {
  userID: number;
  email?: string;
  firstName?: string;
  lastName?: string;
  phone?: string;
  dateOfBirth?: string;
  password?: string;
  username?: string;
  profileImage?: string;
  roleID?: number | null;
}

export interface UserUpdateResponse {
  id: number;
}

@Injectable({
  providedIn: 'root'
})
export class UserUpdateEndpointService {
  private apiUrl = 'http://localhost:7000/api/UserUpdateEndpoint';

  constructor(private http: HttpClient) {}

  updateUser(request: UserUpdateRequest): Observable<UserUpdateResponse> {
    return this.http.put<UserUpdateResponse>(`${this.apiUrl}`, request);
  }
}
