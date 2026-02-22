import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-manage-users',
  templateUrl: './manage-users.component.html',
  styleUrls: ['./manage-users.component.css']
})
export class ManageUsersComponent implements OnInit {

  users: any[] = [];
  lastFiveUsers: any[] = [];
  totalUsers: number = 0;
  showAllUsers = false;


  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  toggleAllUsers() {
  this.showAllUsers = !this.showAllUsers;
}

  loadUsers() {
    debugger
    this.userService.getAllUsers().subscribe({
      next: (data) => {

        this.users = data.sort((a, b) =>
          new Date(b.createdDate).getTime() -
          new Date(a.createdDate).getTime()
        );

        this.totalUsers = this.users.length;

        this.lastFiveUsers = this.users.slice(0, 5);
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
