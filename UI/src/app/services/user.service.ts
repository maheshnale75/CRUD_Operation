import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl = 'https://localhost:7080/api/User';

  constructor(private http: HttpClient) {}

  getAllUsers() {
    return this.http.get<any[]>(`${this.baseUrl}/GetAllUsers`);
  }

}
