import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { AdminHallModule } from './admin-hall/admin-hall.module';
import { HallListComponent } from '../shared/hall/hall-list/hall-list.component';
import { HallAddComponent } from '../shared/hall/hall-add/hall-add.component';

const routes: Routes = [
  {
    path: '',
    component: AdminDashboardComponent,
    children: [
      { path: 'hall', loadChildren: () => import('../shared/hall/hall.module').then(m => m.HallModule) },
      //{ path: '', redirectTo: 'hall/list', pathMatch: 'full' },
      { path: 'fault', loadChildren: () => import('../shared/fault/fault.module').then(m => m.FaultModule) },
      //{ path: '', redirectTo: 'fault/list', pathMatch: 'full' }
      { path: 'equipment', loadChildren: () => import('../shared/equipment/equipment.module').then(m => m.EquipmentModule) },
            { path:'chat',loadChildren:()=>import('../shared/chat/chat-module').then(m=>m.ChatModule)},
      { path: '', redirectTo: 'admin-dashboard', pathMatch: 'full' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
