import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeDashboardComponent } from './employee-dashboard/employee-dashboard.component';
import { EmployeeRoutingModule } from './employee-routing.module';
import { HallModule } from '../shared/hall/hall.module';
import { FaultModule } from '../shared/fault/fault.module';
import { EquipmentModule } from '../shared/equipment/equipment.module';
import { ChatModule } from '../shared/chat/chat-module';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
EmployeeDashboardComponent
  ],
  imports: [
    CommonModule,
    EmployeeRoutingModule,
    HallModule,
    FaultModule,
    EquipmentModule,
    ChatModule,
    FormsModule
  ]
})
export class EmployeeModule { }
