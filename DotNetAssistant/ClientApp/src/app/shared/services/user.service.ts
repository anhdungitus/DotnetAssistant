import { Injectable } from '@angular/core';
import {BehaviorSubject, Observable, throwError} from "rxjs";
import { catchError, retry } from 'rxjs/operators';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { HttpErrorHandler, HandleError } from '../../http-error-handler.service';
import {Credentials} from "../../account/login-form/login-form.component";


const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json',
    Authorization: 'my-auth-token'
  })
}

@Injectable({
  providedIn: 'root'
})
export class UserService  {
  baseUrl = "api";
  private handleError: HandleError;
  private loggedIn = false;

  constructor(private http: HttpClient, httpErrorHandler: HttpErrorHandler) {
    this.handleError = httpErrorHandler.createHandleError('UserService');
    this.loggedIn = !!localStorage.getItem('auth_token');
  }

  register(model: UserRegistration): Observable<UserRegistration>
  {
    return this.http.post<UserRegistration>(this.baseUrl + "/accounts", model, httpOptions)
      .pipe(
        catchError(this.handleError('register', model))
      );
  }

  login(model: Credentials): Observable<Credentials>
  {
    return this.http.post<Credentials>(this.baseUrl + "/auth/login", model, httpOptions)
      .pipe(
        catchError(this.handleError('login', model))
      );
  }

  isLoggedIn() {
    return this.loggedIn;
  }

  setLogin() {
    this.loggedIn = true;
  }
}

export interface UserRegistration {
  email: string;
  password: string;
  firstName: string;
  lastName:  string;
  location: string;
}
