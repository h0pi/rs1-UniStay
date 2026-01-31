import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FaultListComponent } from './fault-list/fault-list.component';
import { FaultUpdateComponent } from './fault-update/fault-update.component';
import { FaultAddComponent } from './fault-add/fault-add.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FaultRoutingModule } from './fault-routing.module';
import { RouterModule } from '@angular/router';
@NgModule({
  declarations: [
    FaultListComponent,
    FaultUpdateComponent,
    FaultAddComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    FaultRoutingModule
  ],
  exports: [
    FaultListComponent, 
    FaultUpdateComponent, 
    FaultAddComponent]
})
export class FaultModule {}