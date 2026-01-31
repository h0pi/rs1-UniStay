// account.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AccountService {
  private base = 'http://localhost:7000/api/account';
  constructor(private http: HttpClient) {}

  getQuestionsForUser(email: string): Observable<any> {
    return this.http.get(`${this.base}/security/questions-for-user?email=${encodeURIComponent(email)}`);
  }

  verifyAnswers(payload: any) {
    return this.http.post(`${this.base}/security/verify`, payload);
  }

  sendEmailToken(email: string) {
    return this.http.post(`${this.base}/password/send-email-token?email=${encodeURIComponent(email)}`, {});
  }

  resetPassword(payload: { token: string, newPassword: string }) {
    return this.http.post(`${this.base}/password/reset`, payload);
  }

  setSecurityAnswers(payload: any) {
    return this.http.post(`${this.base}/security/set`, payload);
  }

  getAllQuestions() {
    return this.http.get(`${this.base}/security/questions-for-user`);
  }

  saveQuestions(payload:any){
    return this.http.post(`${this.base}/set-questions`,payload);
  }
}