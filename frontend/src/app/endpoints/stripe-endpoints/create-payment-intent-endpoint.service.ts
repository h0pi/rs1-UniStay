import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface CreatePaymentIntentRequest {
  invoiceId: number;
}

export interface PaymentIntentResponse {
  clientSecret: string;
  amount: number;
}

@Injectable({
  providedIn: 'root'
})
export class CreatePaymentIntentEndpointService {

  private apiUrl = 'http://localhost:7000/api/CreatePaymentIntentEndpoint/create-intent';

  constructor(private http: HttpClient) {}

  createPaymentIntent(invoiceId: number) {
    return this.http.post<{
      clientSecret: string;
      amount: number;
    }>(this.apiUrl, {
      invoiceId: invoiceId
    });
  }
}
