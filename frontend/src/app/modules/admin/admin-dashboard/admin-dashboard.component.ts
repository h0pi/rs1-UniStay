import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MyAuthService } from '../../../services/auth-services/my-auth.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  //styleUrls: ['./admin-dashboard.component.scss'],
  standalone:false
})
export class AdminDashboardComponent {

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
    this.router.navigate(['/admin/hall/hall-list']);
  }

  openHallAdd(){
    this.router.navigate(['/admin/hall/hall-add']);
  }

  get userRole(){
    return localStorage.getItem('role');
  }

  openFaultList(){
    this.router.navigate(['/admin/fault/fault-list']);
  }

  openFaultAdd(){
    this.router.navigate(['/admin/fault/fault-add']);
  }

    openEquipmentList(){
    this.router.navigate(['/admin/equipment/equipment-list']);
  }

  openEquipmentAdd(){
    this.router.navigate(['/admin/equipment/equipment-add']);
  }

}