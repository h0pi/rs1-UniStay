import { Component } from '@angular/core';
import { TwoFactorService } from '../../../../endpoints/auth-endpoints/two-factor-endpoint.service';
import { Router } from '@angular/router';
import { MyAuthService } from '../../../../services/auth-services/my-auth.service';

@Component({
  selector: 'app-two-factor-verify',
  templateUrl: './two-factor-verify.component.html',
  standalone:false
})
export class TwoFactorVerifyComponent {
  userId = 0; // set from login response (or route query)
  code = '';

  constructor(private svc: TwoFactorService, private router: Router, private auth: MyAuthService) {
    const t = sessionStorage.getItem('2fa_userId');
    this.userId = t ? +t : 0;
  }

  verify() {
  this.svc.verify(this.userId, this.code).subscribe({
    next: (res: any) => {
      const token = res.token;
      const roleName = res.myAuthInfo?.roleName?.toLowerCase();

      this.auth.setSession(token, roleName as any);

      sessionStorage.removeItem('2fa_userId');
      sessionStorage.removeItem('2fa_email');

      if (roleName === 'administrator') {
        this.router.navigate(['/admin']);
      } else if (roleName === 'student') {
        this.router.navigate(['/student']);
      } else if (roleName === 'employee') {
        this.router.navigate(['/employee']);
      } else {
        this.router.navigate(['/login']);
      }
    },
    error: () => alert("Invalid or expired code")
  });
}

  resend() {
    this.svc.sendCode(this.userId).subscribe(() => alert('Code sent'));
  }
}
