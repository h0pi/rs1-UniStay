import { Component, OnInit } from '@angular/core';
import { ChatService } from '../../../../endpoints/message-endpoints/chat-service';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  standalone: false
})
export class ChatComponent implements OnInit {
  get senderId():number{return Number(localStorage.getItem('id'));}
  //senderId:Number(localStorage.getItem('id'));
  receiverId!: number;
  activeUserName = '';

  conversations:any[] = [];
  messages:any[] = [];
  message = '';
  search = '';
  searchResults:any[]=[];
  isTyping=false;
  typingTimeout:any;

  constructor(private chat: ChatService,private http:HttpClient) {}

  ngOnInit() {
const storedId=localStorage.getItem('id');
//if(storedId){
//  this.senderId=Number(storedId);
//}

if (!storedId) { console.error("❌ Nema 'id' u localStorage – login nije spremio userId!"); return; }
//this.senderId=Number(storedId);
    console.log("Sender ID ",this.senderId);

    this.chat.startConnection(this.senderId);

    this.chat.getConversations(this.senderId)
      .subscribe(res => this.conversations = res);

    this.chat.onMessageReceived((msg) => {
      if (msg.senderId === this.receiverId || msg.senderId === this.senderId) {
        this.messages.push(msg);
       }
    });
    this.chat.onUserTyping((senderId) => {
  if (Number(senderId) === this.receiverId) {
    this.isTyping = true;

    clearTimeout(this.typingTimeout);
    this.typingTimeout = setTimeout(() => {
      this.isTyping = false;
    }, 1500);
  }
});
  }

  openConversation(c:any) {
    this.receiverId = c.userId;
    this.activeUserName = c.displayName;

    this.chat.getMessages(this.senderId, this.receiverId)
      .subscribe(res => this.messages = res);
  }

  send() {

    console.log("Klik na dugme detektovan");
    console.log("senderId:", this.senderId, "receiverId:", this.receiverId, "message:", this.message); 
    console.log("chat servis:", this.chat);

    if (!this.message.trim()) return;

    this.chat.sendMessage(this.senderId, this.receiverId, this.message)
      .subscribe({next:()=>{
        this.messages.push({
          senderId: this.senderId,
          senderName: 'You',
          content: this.message
        });
        this.message = '';
      },
      error: (err) => { console.error("Greška prilikom slanja poruke:", err); }
      });
  }

  onTyping(){
    if(this.receiverId){
      this.chat.sendTyping(this.receiverId,this.senderId);
    }
  }

  

  filteredConversations() {
    return this.conversations.filter(c =>
      c.displayName.toLowerCase().includes(this.search.toLowerCase())
    );
  }

 

searchNewUsers() {
  console.log('Search value: ',this.search);
  if (this.search.length > 2) {
    this.chat.searchUsers(this.search).subscribe(res => {
      this.searchResults = res;
    });
  } else {
    this.searchResults = [];
  }
}

startNewChat(user: any) {
  this.receiverId = user.userId;
  this.activeUserName = user.displayName;
  this.messages = []; // Prazno jer je novi chat
  this.search = '';
  this.searchResults = [];
  
  // Ovdje odmah pozovi backend da vidiš ima li ipak starih poruka
  this.chat.getMessages(this.senderId, this.receiverId)
    .subscribe(res => this.messages = res);
}
}