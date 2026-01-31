import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { HallListComponent } from './hall-list/hall-list.component';
import { HallAddComponent } from './hall-add/hall-add.component';
import { HallRoutingModule } from './hall-routing.module';
import { HallUpdateComponent } from './hall-update/hall-update.component';


@NgModule({
  declarations: [
    HallListComponent,
    HallAddComponent,
    HallUpdateComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    HallRoutingModule
  ],
  exports: [
    HallListComponent,
    HallAddComponent,
    HallUpdateComponent
  ]
})
export class HallModule {}