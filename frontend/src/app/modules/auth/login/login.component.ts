import { Component, NgZone, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MyAuthService } from '../../../services/auth-services/my-auth.service';
import { Router } from '@angular/router';
import { MyInputTextType } from '../../shared/my-reactive-forms/my-input-text/my-input-text.component';
import { UserGetByIdEndpointService } from '../../../endpoints/user-endpoints/user-get-by-id-endpoint.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: false
})
export class LoginComponent implements OnInit {

  form: FormGroup;
  protected readonly MyInputTextType = MyInputTextType;

  constructor(
    private fb: FormBuilder,
    private authService: MyAuthService,
    private router: Router,
    private zone: NgZone,
    private userGetByIdEndpoint: UserGetByIdEndpointService
) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      rememberMe: [false]
    });
  }

  ngOnInit(): void {
    const savedEmail = localStorage.getItem('rememberedEmail');
    const savedPassword = localStorage.getItem('rememberedPassword');


    if (savedEmail && savedPassword) {
      this.form.patchValue({
        email: savedEmail,
        password: savedPassword,
        rememberMe: true
      });
    }
  }

  passwordRecovery(){
      this.router.navigate(['/password-recovery']);
  }

  onLogin(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { email, password, rememberMe } = this.form.value;

    this.authService.login(email, password).subscribe({
      next: (response) => {

        // ⭐⭐⭐ FIXED — 2FA DODANO OVDJE ⭐⭐⭐
        if (response.requiresTwoFactor) {
          localStorage.setItem('id', response.userId.toString());
          sessionStorage.setItem('2fa_userId', response.userId.toString());
          sessionStorage.setItem('2fa_email', response.email);

          this.router.navigate(['/two-factor/two-factor-verify'], {
            queryParams: { email: response.email }
          });

          return; // ⭐ PREKID NORMALNOG LOGIN-A
        }
        // ⭐⭐⭐ KRAJ FIXA ⭐⭐⭐


        // -------------------------------
        // ORIGINALNI LOGIN – NIJE DIRAN
        // -------------------------------
        const authInfo = response.myAuthInfo;
        const token = response.token;
        const roleName = authInfo?.roleName?.toLowerCase();



        const authPayload = { token, ...response };
//localStorage.setItem('my-auth-token', JSON.stringify(authPayload));
localStorage.setItem('token', token);
localStorage.setItem('id', response.userId.toString());
console.log("User ID iz response-a:", response.userId);



        //localStorage.setItem('token',token);
        //localStorage.setItem('id',response.userId.toString());


//try { const payload = JSON.parse(atob(token.split('.')[1])); const senderId = payload.nameid || payload.sub; if (senderId) { localStorage.setItem('senderId', senderId); console.log("SenderId iz tokena:", senderId); } } catch (e) { console.error("Greška pri dekodiranju tokena:", e); }

        this.authService.setSession(token, roleName as any);
        localStorage.setItem('role', roleName);

        this.userGetByIdEndpoint.getById(response.userId)
          .subscribe(user => {
            this.authService.setUser(user);
          });



        if (rememberMe) {
          localStorage.setItem('rememberedEmail', email);
          localStorage.setItem('rememberedPassword', password);
        } else {
          localStorage.removeItem('rememberedEmail');
          localStorage.removeItem('rememberedPassword');
        }

        if (roleName === 'administrator') {
          this.router.navigate(['/admin']);
        } else if (roleName === 'student') {
          this.router.navigate(['/student']);
        } else if (roleName === 'employee') {
          this.router.navigate(['/employee']);
        } else {
          this.router.navigate(['/login']);
        }
      }/*,
      error: (err) => {
        console.error('❌ Login failed:', err);
        alert('Pogrešan email ili lozinka!');
      }*/

    });
  }
}
