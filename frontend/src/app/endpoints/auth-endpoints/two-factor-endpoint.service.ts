import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({providedIn:'root'})
export class TwoFactorService {
  private base = 'http://localhost:7000/api/account/2fa';

  constructor(private http: HttpClient) {}

  enable(userId:number): Observable<any> {
    return this.http.post(`${this.base}/enable`, { userId });
  }
  disable(userId:number): Observable<any> {
    return this.http.post(`${this.base}/disable`, { userId });
  }
  sendCode(userId:number): Observable<any> {
    return this.http.post(`${this.base}/send-code`, { userId });
  }
  verify(userId:number, code:string): Observable<any> {
    return this.http.post(`${this.base}/verify`, { userId, code });
  }
}