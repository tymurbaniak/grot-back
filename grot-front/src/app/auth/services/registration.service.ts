import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

  private userSubject: BehaviorSubject<User>;  

  public user: Observable<User>;
  
  constructor(
    private router: Router,
    private http: HttpClient
  ) {
    this.userSubject = new BehaviorSubject<User>(new User());
    this.user = this.userSubject.asObservable();
   }
  
  register(username: string, password: string, email: string) {
    return this.http.post<any>(`${environment.apiUrl}/register/newuser`, { username, password, email }, { withCredentials: true })
      .pipe(map((user: User) => {
        this.userSubject.next(user);
        return user;
      }));
  }

  
}
