// src/app/app-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthLayoutComponent } from './modules/auth/auth-layout/auth-layout.component';
import { LoginComponent } from './modules/auth/login/login.component';

const routes: Routes = [
  // public/auth routes (bez dashboard layout)
  {
    path: 'auth',
    //component: AuthLayoutComponent,
    loadChildren: ()=>
      import('./modules/auth/auth-module').then(m => m.AuthModule),
       // path: 'login', component: LoginComponent ,
       // path: '', redirectTo: 'login', pathMatch: 'full'

  },
  {
    path:'two-factor',
    loadChildren:()=>
      import('./modules/shared/two-factor/two-factor.module')
      .then(m=>m.TwoFactorModule)
  },
  {
    path:'chat',
    loadChildren:()=>
      import('./modules/shared/chat/chat-module').then(m=>m.ChatModule)
  },

   {
    path:'analytics',
    loadChildren:()=>
      import('./modules/shared/analytics/analytics-module').then(m=>m.AnalyticsModule)
  },

  {
    path:'',
    pathMatch:'full',
    loadChildren:()=>
      import('./modules/shared/shared.module').then(m=>m.SharedModule)
  },


  // protected dashboards (svaki korisnik ima svoj dashboard modul)
  { path: 'student', loadChildren: () => import('./modules/student/student-module').then(m => m.StudentModule) },
  { path: 'employee', loadChildren: () => import('./modules/employee/employee-module').then(m => m.EmployeeModule) },
  { path: 'admin', loadChildren: () => import('./modules/admin/admin-module').then(m => m.AdminModule) },

 /*  {
  path: 'chat',
  loadChildren: () =>
    import('./modules/shared/chat/chat-module').then(m => m.ChatModule)
  },*/

  // fallback
  { path: '**', redirectTo: 'auth/login', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
