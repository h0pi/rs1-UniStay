import {Component, signal} from '@angular/core';
import { Router } from '@angular/router';
import { MyAuthService } from '../../../services/auth-services/my-auth.service';
import { UserGetByIdEndpointService } from '../../../endpoints/user-endpoints/user-get-by-id-endpoint.service';
import {Observable} from 'rxjs';


@Component({
  selector: 'app-student-dashboard',
  templateUrl: './student-dashboard.component.html',
  styleUrls: ['./student-dashboard.component.scss'],
  standalone:false
})
export class StudentDashboardComponent {



  constructor(
    private authService: MyAuthService,
    private router: Router,
    private userGetByIdEndpoint: UserGetByIdEndpointService
  ) {console.log("Dashboard je ucitan");}

  readonly navItems = signal([
    { path: 'dashboard', icon: 'fa-solid fa-table-columns', label: 'Dashboard' },
    { path: '/student/invoices', icon: 'fa-solid fa-file-invoice-dollar', label: 'Invoices' },
    { path: 'payments', icon: 'fa-solid fa-clock-rotate-left', label: 'Payments History' },
    { path: 'profile', icon: 'fa-solid fa-user', label: 'Profile' }
  ]);
  user$!: Observable<any>;

  ngOnInit() {
    this.user$ = this.authService.user$;
    if (!this.authService.getUser()) {
      const id = localStorage.getItem('id');
      if (id) {
        this.userGetByIdEndpoint.getById(+id)
          .subscribe(user => this.authService.setUser(user));
      }
    }
  }
  logout() {
    this.authService.logout().subscribe(() => {
      this.router.navigate(['/auth/login']);
    });
  }
}
