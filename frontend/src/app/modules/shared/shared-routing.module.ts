import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {ErrorPageComponent} from './error-page/error-page';
import {MyInputTextComponent} from './my-reactive-forms/my-input-text/my-input-text.component';
import { HallModule } from './hall/hall.module';
import { LandingPageComponent } from './pages/landing/landing.component';
import { AboutComponent } from './pages/about/about.component';
import {FeaturesComponent} from './pages/features/features.component';
import {MealPlanComponent} from './pages/meal-plan/meal-plan.component';
import {StaffComponent} from './pages/staff/staff.component';
import { FaultModule } from './fault/fault.module';
import { EquipmentModule } from './equipment/equipment.module';
import { PasswordRecoveryComponent } from './password-recovery/password-recovery.component';
import { SetSecurityQuestionsComponent } from './set-security/set-security-questions.component';
import { TwoFactorModule } from './two-factor/two-factor.module';
import { ChatModule } from './chat/chat-module';
import { AnalyticsModule } from './analytics/analytics-module';

const routes: Routes = [
  {path: '', component: LandingPageComponent},
  {path: 'about', component: AboutComponent},
  {path: 'features', component: FeaturesComponent},
  {path: 'meal-plan', component: MealPlanComponent},
  {path: 'staff', component: StaffComponent},
  {path: 'error-page',component:ErrorPageComponent},
  {path: 'my-input-text',component:MyInputTextComponent},
  {path: 'hall',component:HallModule},
  {path: 'fault', component:FaultModule},
  {path: 'equipment', component:EquipmentModule},
  {path: 'password-recovery',component:PasswordRecoveryComponent},
  {path: 'set-security-questions',component:SetSecurityQuestionsComponent},

 // {path: 'two-factor',component:TwoFactorModule}
  {
  path: 'two-factor',
  loadChildren: () =>
    import('./two-factor/two-factor.module').then(m => m.TwoFactorModule)
 },

  {
  path: 'chat',
  loadChildren: () =>
    import('./chat/chat-module').then(m => m.ChatModule)
  },
{
    path: 'analytics',
  loadChildren: () =>
    import('./analytics/analytics-module').then(m => m.AnalyticsModule)
  }

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SharedRoutingModule { }
