import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StudentRoutingModule } from './student-routing.module';
import { StudentDashboardComponent } from './student-dashboard/student-dashboard.component';
import { RouterModule } from '@angular/router';
import { ChatModule } from '../shared/chat/chat-module';
import { FormsModule } from '@angular/forms';
import {InvoicesComponent} from './pages/invoices/invoices.component';

@NgModule({
  declarations: [
    StudentDashboardComponent,
    InvoicesComponent,
  ],
  imports: [
    CommonModule,
    StudentRoutingModule,
    RouterModule,
    ChatModule,
    FormsModule
  ]
})
export class StudentModule { }
