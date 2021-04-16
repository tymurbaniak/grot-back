import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../models/user';
import { map } from 'rxjs/operators';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private userSubject: BehaviorSubject<User>;
  private refreshTokenTimeout: any;

  public user: Observable<User>;

  constructor(
    private router: Router,
    private http: HttpClient
  ) {
    this.userSubject = new BehaviorSubject<User>(new User());
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): User {
    return this.userSubject.value;
  }

  public get isAuthenticated(): boolean {
    return this.userSubject.value.id !== -1;
  }

  login(username: string, password: string) {
    return this.http.post<any>(`${environment.apiUrl}/auth/authenticate`, { username, password }, { withCredentials: true })
      .pipe(map((user: User) => {
        this.userSubject.next(user);
        this.startRefreshTokenTimer();
        return user;
      }));
  }

  logout() {
    this.http.post<any>(`${environment.apiUrl}/auth/revoke-token`, {}, { withCredentials: true }).subscribe();
    this.stopRefreshTokenTimer();
    this.userSubject.next(new User());
    this.router.navigate(['/login']);
  }

  refreshToken() {
    return this.http.post<any>(`${environment.apiUrl}/auth/refresh-token`, {}, { withCredentials: true })
      .pipe(map((user: User) => {
        this.userSubject.next(user);
        this.startRefreshTokenTimer();
        return user;
      }));
  }

  private startRefreshTokenTimer() {
    // parse json object from base64 encoded jwt token
    if (this.hwtTokenExists(this.userValue.jwtToken)) {
      const jwtToken = JSON.parse(atob(this.userValue.jwtToken.split('.')[1]));
      // set a timeout to refresh the token a minute before it expires
      const expires = new Date(jwtToken.exp * 1000);
      const timeout = expires.getTime() - Date.now() - (60 * 1000);
      this.refreshTokenTimeout = setTimeout(() => this.refreshToken().subscribe(), timeout);
    }
  }

  private hwtTokenExists = (token: any): token is string => token !== undefined;

  private stopRefreshTokenTimer() {
    clearTimeout(this.refreshTokenTimeout);
  }
}

