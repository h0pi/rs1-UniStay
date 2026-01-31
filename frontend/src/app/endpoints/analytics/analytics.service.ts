import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AnalyticsService {
  
private hubConnection?: signalR.HubConnection;
  private analyticsSource = new BehaviorSubject<any>({
    activeUsers: 0,
    totalUsers: 0,
    totalMessages: 0
  });
  analytics$ = this.analyticsSource.asObservable();

constructor() {
  window.addEventListener('beforeunload', () => {
    this.stopConnection();
  });
}


startConnection(userId: number) {
  if (this.hubConnection) {
    return;
  }

  this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(`http://localhost:7000/hubs/analytics?userId=${userId}`)
    .withAutomaticReconnect()
    .build();

  this.hubConnection.on('AnalyticsUpdated', data => {
    this.analyticsSource.next(data);
  });

  this.hubConnection.start();
}




stopConnection() {
  if (this.hubConnection) {
    this.hubConnection.stop();
    this.hubConnection = undefined;
  }
}

}