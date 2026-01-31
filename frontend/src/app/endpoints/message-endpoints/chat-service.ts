import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ChatService {

  private hubConnection!: signalR.HubConnection;
  private messageReceived$ = new Subject<any>();
  private typing$=new Subject<string>();

  constructor(private http: HttpClient) {}

//startConnection() {
startConnection(userId: number) {
  this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(`http://localhost:7000/hubs/chat?userId=${userId}`)
    .withAutomaticReconnect()
    .build();

  this.hubConnection.on('ReceiveMessage', (msg) => {
    this.messageReceived$.next(msg);
  });

  this.hubConnection.on('UserTyping',(senderId)=>{
    this.typing$.next(senderId);
  })

  this.hubConnection.start()
    .then(() => console.log('✅ SignalR CONNECTED'))
    .catch(err => console.error('❌ SignalR ERROR', err));
  this.hubConnection.on('UserTyping',(senderId:string)=>{
    this.typing$.next(senderId);
  })
}

onUserTyping(callback:(senderId:string)=>void){
  this.typing$.subscribe(callback);
}


searchUsers(username: string) {
  return this.http.get<any[]>(`http://localhost:7000/api/chat/search-users?username=${username}`);
}

/*

const token=localStorage.getItem('token');
console.log('Token koji šaljem SingalR-u: ',token);

  this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:7000/hubs/chat', {
      accessTokenFactory: () => localStorage.getItem('token') || ''
    })
    .withAutomaticReconnect()
    .build();

  this.hubConnection.on('ReceiveMessage', (msg) => {
    this.messageReceived$.next(msg);
  });

  this.hubConnection.start()
    .then(() => console.log('✅ SignalR CONNECTED'))
    .catch(err => console.error('❌ SignalR ERROR', err));
}*/


sendTyping(receiverId: number,senderId:number) {
  if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
    this.hubConnection.invoke('Typing', receiverId,senderId);
  }
}

  onMessageReceived(callback: (msg:any)=>void) {
    this.messageReceived$.subscribe(callback);
  }

  getConversations(userId:number) {
    return this.http.get<any[]>(`http://localhost:7000/api/chat/conversations/${userId}`);
  }

  getMessages(userId:number, otherUserId:number) {
    return this.http.get<any[]>(`http://localhost:7000/api/chat/messages?userId=${userId}&otherUserId=${otherUserId}`);
  }

  sendMessage(senderId:number, receiverId:number, content:string) {
    return this.http.post(`http://localhost:7000/api/messages/send`, {
      senderUserID: senderId,
      receiverUserID: receiverId,
      messageText: content
    });
  }
}