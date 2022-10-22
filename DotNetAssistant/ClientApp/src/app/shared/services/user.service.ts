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
export class UserService {
  baseUrl = "api";
  private handleError: HandleError;

  constructor(private http: HttpClient, httpErrorHandler: HttpErrorHandler) {
    this.handleError = httpErrorHandler.createHandleError('UserService');
  }

  register(model: UserRegistration): Observable<UserRegistration>
  {
    return this.http.post<UserRegistration>(this.baseUrl + "/accounts", model, httpOptions)
      .pipe(
        catchError(this.handleError('register', model))
      );
  }

  // login(model: Credentials) {
  //   let headers = new Headers();
  //   headers.append('Content-Type', 'application/json');
  //
  //   return this.http.post<Credentials>(this.baseUrl + '/auth/login', model, httpOptions)
  //     .subscribe(res => {
  //       localStorage.setItem('auth_token', res.auth_token);
  //       this.loggedIn = true;
  //       this._authNavStatusSource.next(true);
  //       return true;
  //     })
  //     .catch(this.handleError);
  // }
}

export interface UserRegistration {
  email: string;
  password: string;
  firstName: string;
  lastName:  string;
  location: string;
}
