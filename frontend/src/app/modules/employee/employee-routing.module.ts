// src/app/modules/employee/employee-routing.module.ts
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmployeeDashboardComponent } from './employee-dashboard/employee-dashboard.component';

const routes: Routes = [
  {
    path: '',
    component: EmployeeDashboardComponent,
    children: [
      { path: 'hall', loadChildren: () => import('../shared/hall/hall.module').then(m => m.HallModule) },
      //{ path: '', redirectTo: 'hall/list', pathMatch: 'full' },
      { path: 'fault', loadChildren: () => import('../shared/fault/fault.module').then(m => m.FaultModule) },
      //{ path: '', redirectTo: 'fault/list', pathMatch: 'full' }
      { path: 'equpiment', loadChildren: () => import('../shared/equipment/equipment.module').then(m => m.EquipmentModule) },
      { path:'chat',loadChildren:()=>import('../shared/chat/chat-module').then(m=>m.ChatModule)},
      { path: '', redirectTo: 'employee-dashboard', pathMatch: 'full' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployeeRoutingModule {}