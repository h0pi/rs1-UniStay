import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MyAuthService } from '../../../services/auth-services/my-auth.service';

@Component({
  selector: 'app-employee-dashboard',
  templateUrl: './employee-dashboard.component.html',
  //styleUrls: ['./employee-dashboard.component.scss'],
  standalone:false
})
export class EmployeeDashboardComponent {

  constructor(
    private authService: MyAuthService,
    private router: Router
  ) {console.log("Dashboard je ucitan");}

  

  logout() {
    this.authService.logout().subscribe(() => {
      this.router.navigate(['/auth/login']);
    });
  }

    openHallList(){
    this.router.navigate(['/employee/hall/hall-list']);
  }

  openHallAdd(){
    this.router.navigate(['/employee/hall/hall-add']);
  }

  get userRole(){
    return localStorage.getItem('role');
  }

  openFaultList(){
    this.router.navigate(['/employee/fault/fault-list']);
  }

  openFaultAdd(){
    this.router.navigate(['/employee/fault/fault-add']);
  }

  openEquipmentList(){
    this.router.navigate(['/employee/equipment/equipment-list']);
  }

  openEquipmentAdd(){
    this.router.navigate(['/employee/equipment/equipment-add']);
  }

    openChat(){
    this.router.navigate(['/employee/chat/chat-component']);
  }
}