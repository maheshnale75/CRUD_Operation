import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
    Input = '';
    password = '';

    errorMessage = '';
    successMessage = '';

    constructor(private authService: AuthService, private router: Router) {}

    login() {
      this.errorMessage='';
      this.successMessage='';

      const payload = {
       Input : this.Input,
       password : this.password
      };

      this.authService.login(payload).subscribe({
        next: (res) => {
          this.authService.saveToken(res.token);
          this.successMessage = 'Login Successfull';
          this.router.navigate(['/home']);
        },
        error: (err) => {
          this.errorMessage = err.error?.message || 'Login Failed';
        }
      });

    }
}
