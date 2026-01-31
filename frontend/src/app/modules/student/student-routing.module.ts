import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentDashboardComponent } from './student-dashboard/student-dashboard.component';
import {InvoicesComponent} from './pages/invoices/invoices.component';
import {AuthGuard} from '../../auth-guards/auth-guard.service';


// const routes: Routes = [
// {path:' ',component:StudentDashboardComponent},
// {path:'student-dashboard',component:StudentDashboardComponent},
// { path:'chat',loadChildren:()=>import('../shared/chat/chat-module').then(m=>m.ChatModule)},
// ];
const routes: Routes = [
  {
    path: '',
    component: StudentDashboardComponent,
    //canActivateChild: [AuthGuard],
    children: [
      //{ path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      //{ path:'chat',loadChildren:()=>import('../shared/chat/chat-module').then(m=>m.ChatModule)},
      //{ path: 'dashboard', component: DashboardHomeComponent },
      { path: 'invoices', component: InvoicesComponent },
      //{ path: 'payments', component: PaymentsComponent },
      //{ path: 'profile', component: ProfileComponent }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StudentRoutingModule { }
