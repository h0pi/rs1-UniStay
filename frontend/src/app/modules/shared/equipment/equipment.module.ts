import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { EquipmentListComponent } from './equipment-list/equipment-list.component';
import { EquipmentAddComponent } from './equipment-add/equipment-add.component';
import { EquipmentRoutingModule } from './equipment-routing.module';
import { EquipmentUpdateComponent } from './equipment-update/equipment-update.component';
import { EquipmentItemsListComponent } from './equipment-items-list/equipment-items-list.component';
import { EquipmentItemCreateComponent } from './equipment-items-create/equipment-items-create.component';
import { EquipmentItemUpdateComponent } from './equipment-items-update/equipment-items-update.component';


@NgModule({
  declarations: [
    EquipmentListComponent,
    EquipmentAddComponent,
    EquipmentUpdateComponent,
    EquipmentItemsListComponent,
    EquipmentItemCreateComponent,
    EquipmentItemUpdateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    EquipmentRoutingModule
  ],
  exports: [
    EquipmentListComponent,
    EquipmentAddComponent,
    EquipmentUpdateComponent,
    EquipmentItemsListComponent,
    EquipmentItemCreateComponent,
    EquipmentItemUpdateComponent
  ]
})
export class EquipmentModule {}