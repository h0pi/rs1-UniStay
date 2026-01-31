import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserModule } from "../../shared/user/user.module";

@NgModule({
  imports: [
    CommonModule,
    UserModule,
    RouterModule.forChild([
      {
        path: '',
        loadChildren: () =>
          import('../../shared/user/user.module').then(m => m.UserModule)
      }
    ])
  ]
})
export class AdminUserModule {}
