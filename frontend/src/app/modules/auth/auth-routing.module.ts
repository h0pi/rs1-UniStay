import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from './login/login.component';
import {RegisterComponent} from './register/register.component';
import {AuthLayoutComponent} from './auth-layout/auth-layout.component';
import {ErrorPageComponent} from '../shared/error-page/error-page';
import { LogoutComponent } from './logout/logout.component';
import { PasswordRecoveryComponent } from '../shared/password-recovery/password-recovery.component';


const routes: Routes = [
  { path:'',component:AuthLayoutComponent,children:[
      {path:'',redirectTo:'login',pathMatch:'full'},
      {path:'login',component:LoginComponent},
      {path:'logout',component:LogoutComponent},
      {path:'register',component:RegisterComponent},
      {path:'**',component:ErrorPageComponent}
]},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
