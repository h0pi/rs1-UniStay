import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { HallModule } from './hall/hall.module';
import { FaultModule } from './fault/fault.module';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { LandingPageComponent } from './pages/landing/landing.component';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { SharedRoutingModule} from './shared-routing.module';
import { AnimateOnScrollDirective } from './directives/animate-on-scroll.directive';
import { AboutComponent } from './pages/about/about.component';
import { FeaturesComponent } from './pages/features/features.component';
import { StaffComponent } from './pages/staff/staff.component';
import { MealPlanComponent } from './pages/meal-plan/meal-plan.component';
import { UserAddComponent } from './user/user-add/user-add.component';
import { EquipmentModule } from './equipment/equipment.module';
import { PasswordRecoveryComponent } from './password-recovery/password-recovery.component';
import { SetSecurityQuestionsComponent } from './set-security/set-security-questions.component';
import { TwoFactorModule } from './two-factor/two-factor.module';
import { AnalyticsModule } from './analytics/analytics-module';



@NgModule({
  declarations: [
    FooterComponent,
    HeaderComponent,
    LandingPageComponent,
    AnimateOnScrollDirective,
    AboutComponent,
    FeaturesComponent,
    StaffComponent,
    MealPlanComponent,
    AboutComponent,
    UserAddComponent,
    PasswordRecoveryComponent,
    SetSecurityQuestionsComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    SharedRoutingModule,
    FormsModule//,
    //ChatModule
  ],
  exports: [
    CommonModule,
    ReactiveFormsModule,
    HallModule,
    FaultModule,
    HeaderComponent,
    AboutComponent,
    FooterComponent,
    FeaturesComponent,
    StaffComponent,
    MealPlanComponent,
    UserAddComponent,
    PasswordRecoveryComponent,
    EquipmentModule,
    TwoFactorModule,
    AnalyticsModule
    //ChatModule
   // SetSecurityQuestionsComponent
  ]
})
export class SharedModule {}
