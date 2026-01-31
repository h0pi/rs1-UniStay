import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Invoice {
  invoiceID: number;
  totalAmount: number;
  paid: boolean;
  issuedAt: boolean;
}
@Injectable({
  providedIn: 'root'
})
export class GetInvoicesByUserIdEndpointService {

  private apiUrl = 'http://localhost:7000/api/GetInvoicesByUserIdEndpoint';

  constructor(private http: HttpClient) {}

  getByUserId(userId: number): Observable<Invoice[]> {
    return this.http.get<Invoice[]>(`${this.apiUrl}/${userId}`);
  }
}
