import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GetInvoicePdfEndpointService {

  // direktno backend URL (kako ti već koristiš svugdje)
  private readonly apiUrl = 'http://localhost:7000/api/GetInvoicePdfEndpoint';

  constructor(private http: HttpClient) {}

  downloadInvoicePdf(invoiceId: number): Observable<Blob> {
    return this.http.get(
      `${this.apiUrl}/${invoiceId}/pdf`,
      {
        responseType: 'blob'
      }
    );
  }
}
