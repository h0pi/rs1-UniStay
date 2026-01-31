import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { LoginTokenDto } from './dto/login-token-dto';


export type UserRole = 'employee' | 'student' | 'admin';

@Injectable({
  providedIn: 'root'
})
export class MyAuthService {

  private apiUrl = 'http://localhost:7000/api'; // <-- PRAVI BACKEND URL

  private loggedInUser: any = null; // ✅ dodano

  constructor(private http: HttpClient) { }

  // ✅ Dodano — čuva korisnika u memoriji i localStorage
  setLoggedInUser(user: any) {
    this.loggedInUser = user;

    if (user?.token) {
      localStorage.setItem('token', user.token);
    }

    if (user?.role) {
      localStorage.setItem('role', user.role);
    }

    if (user?.email) {
      localStorage.setItem('email', user.email);
    }
  }

  // ✅ Dodano — vraća ulogovanog korisnika
  getLoggedInUser() {
    return this.loggedInUser;
  }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/auth/login`,
      { email, password }
    );
  }

logout() {
  const token = localStorage.getItem('token');

  const headers = new HttpHeaders({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });

  return this.http.post('http://localhost:7000/api/auth/logout', {}, { headers });
}

  setSession(token: string, role: string) {
    localStorage.setItem('token', token);
    localStorage.setItem('role', role);

  }

  clearSession(){
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    localStorage.removeItem('email');
    this.loggedInUser=null;
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  isEmployee(): boolean {
    return localStorage.getItem('role') === 'employee';
  }

  isStudent(): boolean {
    return localStorage.getItem('role') === 'student';
  }

  isAdmin(): boolean {
    return localStorage.getItem('role') === 'admin';
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRole(): UserRole | null {
    return localStorage.getItem('role') as UserRole | null;
  }

  private userSubject = new BehaviorSubject<any | null>(null);
  user$ = this.userSubject.asObservable();

  setUser(user: any) {
    this.userSubject.next(user);
  }

  getUser() {
    return this.userSubject.value;
  }
}
