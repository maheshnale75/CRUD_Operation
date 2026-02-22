import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7080/api/Auth';
  constructor(private http: HttpClient) { }

  register(data: any) {
  return this.http.post(`${this.apiUrl}/registeruser`, data);
}
  login(data: any) {
    return this.http.post<any>(`${this.apiUrl}/Login`, data);
  }
  saveToken(token: string) {
    localStorage.setItem('token', token);

    const decoded: any = jwtDecode(token);

    localStorage.setItem('username', decoded.given_name);
    localStorage.setItem('role', decoded.role);
    localStorage.setItem('sessionId', decoded.nameid);

  }
  getUsername() {
    return localStorage.getItem('username');
  }
  getRole() {
    return localStorage.getItem('role');
  }
  logout(): Observable<any> {
        const sessionId = localStorage.getItem('sessionId');
        const headers = new HttpHeaders({
    'Content-Type': 'application/json'
  });
         return this.http.post(
    `${this.apiUrl}/LogOut`,
    JSON.stringify(sessionId),
    { headers }
  );
  }
  clearStorage() {
    localStorage.clear();
  }
  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
  
}
