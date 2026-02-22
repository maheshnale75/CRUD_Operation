import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent{
  username = localStorage.getItem('username') || '';
  role = localStorage.getItem('role') || '';
  showMenu = false;
  showAdminMenu = false;
  isAdminOrHR = false;

  constructor(private router: Router, private authService: AuthService) {}
  
   ngOnInit(): void {
    this.isAdminOrHR = this.role === 'Admin' || this.role === 'Hr Manager';
  }
  get firstLetter(): string {
    return this.username.charAt(0).toUpperCase();
  }
  
  toggleMenu() {
    this.showMenu = !this.showMenu;
  }

  toggleAdminMenu() {
    this.showAdminMenu = !this.showAdminMenu;
  }

  goToManageUsers() {
    this.showAdminMenu = false;
    this.router.navigate(['/manage-users']);
  }

  logout() {
  this.authService.logout().subscribe({
    next: () => {
      this.authService.clearStorage();
      this.router.navigate(['/login']);
    },
    error: () => {
      this.authService.clearStorage();
      this.router.navigate(['/login']);
    }
  });
  }
}
