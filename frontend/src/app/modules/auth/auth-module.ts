import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthLayoutComponent } from './auth-layout/auth-layout.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { LogoutComponent } from './logout/logout.component';
import { RouterModule } from '@angular/router';
import { AuthRoutingModule } from './auth-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from '../shared/shared.module';
import { MyReactiveFormsModule } from '../shared/my-reactive-forms/my-reactive-forms.module';
import { AdminHallModule } from '../admin/admin-hall/admin-hall.module';
import { EmployeeModule } from '../employee/employee-module';
import { PasswordRecoveryComponent } from '../shared/password-recovery/password-recovery.component';
import { SecurityQuestionsComponent } from './security-questions/security-questions.component';
import { ChatModule } from '../shared/chat/chat-module';
import { AnalyticsModule } from '../shared/analytics/analytics-module';

@NgModule({
  declarations: [
    AuthLayoutComponent,
    LoginComponent,
    LogoutComponent,
    RegisterComponent,
    SecurityQuestionsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    AuthRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatSlideToggleModule,
    MatButtonModule,
    RouterModule,
    MyReactiveFormsModule
  ]
})
export class AuthModule {}
