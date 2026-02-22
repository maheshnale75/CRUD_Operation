import { Component } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  username = '';
  password = '';
  emailId = '';
  roleId: number = 0;
  mobileNumber = '';
  country = '';
  createdDate: string = new Date().toISOString();
  selectedFile!: File;
  successMessage = '';
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }
  
  register() {

    const formData = new FormData();

    formData.append('UserName', this.username);
    formData.append('Password', this.password);
    formData.append('EmailId', this.emailId);
    formData.append('RoleId', this.roleId.toString());
    formData.append('MobileNumber', this.mobileNumber);
    formData.append('Country', this.country);
    formData.append('CreatedDate', this.createdDate);

    if (this.selectedFile) {
      formData.append('ProfileImage', this.selectedFile);
    }

    this.authService.register(formData).subscribe({
      next: () => {
        this.successMessage = "User Registered Successfully!";
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1500);
      },
      error: (err) => {
        this.errorMessage = err.error?.message || "Registration Failed";
      }
    });
  }
}
