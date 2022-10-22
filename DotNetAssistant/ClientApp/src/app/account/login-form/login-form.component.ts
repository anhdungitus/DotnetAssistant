import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subscription} from "rxjs";
import {UserService} from "../../shared/services/user.service";
import {ActivatedRoute, Router} from "@angular/router";
import {FormBuilder, FormControl, Validators} from "@angular/forms";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})

export class LoginFormComponent implements OnInit, OnDestroy {
  errors: string = "";
  loginForm = this.fb.group({
    username: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });
  constructor(private fb: FormBuilder, private userService: UserService, private router: Router,private activatedRoute: ActivatedRoute) { }

  ngOnDestroy(): void {
  }

  ngOnInit(): void {
  }

  onSubmit() {
    this.userService.login(this.loginForm.value)
      .subscribe(
        result  => {
          if(result){
          console.log(result);
          localStorage.setItem('auth_token', result.auth_token);
          this.userService.setLogin();
          this.router.navigate(['']).then(r => r);
        }},
        errors =>  this.errors = errors);
  }

}

export interface Credentials {
  username: string;
  password: string;
  id: string;
  auth_token: string;
}

export interface JwtResponse {

}


