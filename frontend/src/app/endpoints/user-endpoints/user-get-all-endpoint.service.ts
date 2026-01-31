import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface UserListItem {
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

export interface UserGetAllResponse {
  items: UserListItem[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

@Injectable({
  providedIn: 'root'
})
export class UserGetAllEndpointService {
  private apiUrl = 'http://localhost:7000/api/UserGetAllEndpoint';

  constructor(private http: HttpClient) {}

  getAll(filter: any): Observable<UserGetAllResponse> {
    const params = new HttpParams({ fromObject: filter });
    return this.http.get<UserGetAllResponse>(this.apiUrl, { params });
  }
}
