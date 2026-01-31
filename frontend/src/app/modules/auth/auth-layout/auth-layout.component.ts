import { Component } from '@angular/core';
import { MyAuthService } from '../../../services/auth-services/my-auth.service';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';


@Component({
  selector: 'app-auth-layout',
  templateUrl: './auth-layout.component.html',
  standalone: false
})
export class AuthLayoutComponent {
  isLoginPage = false;

  constructor(
    private authService: MyAuthService,
    private router: Router
  ) {

    this.isLoginPage=this.router.url.includes('/login');
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.isLoginPage = event.url.includes('/login');
      });
  }

ngOnInit(): void {
  const currentUrl = this.router.url;
  this.isLoginPage =
    currentUrl.includes('/login') ||
    currentUrl.includes('/auth/login');

  this.router.events.subscribe(event => {
    if (event instanceof NavigationEnd) {
      const url = event.urlAfterRedirects || event.url;
      this.isLoginPage =
        url.includes('/login') ||
        url.includes('/auth/login');
    }
  });
}

  logout() {
  this.authService.logout().subscribe({
    next: () => {
      this.authService.clearSession();
      this.router.navigate(['/login']);
    },
    error: () => {
      this.authService.clearSession();
      this.router.navigate(['/login']);
    }
  });
}

    openHallList(){
    this.router.navigate(['hall-list']);
  }

  openHallAdd(){
    this.router.navigate(['hall-add']);
  }

  get userRole(){
    return localStorage.getItem('role');
  }

  openFaultList(){
    this.router.navigate(['fault-list']);
  }

  openFaultAdd(){
    this.router.navigate(['fault-add']);
  }

    openEquipmentList(){
    this.router.navigate(['equipment-list']);
  }

  openEquipmentAdd(){
    this.router.navigate(['equipment-add']);
  }

openChat(){
  this.router.navigate(['chat']);
}

analyticsDashboard(){
  this.router.navigate(['analytics-dashboard']);
}
}
