import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import {UserRegistration, UserService} from '../../shared/services/user.service';

@Component({
  selector: 'app-registration-form',
  templateUrl: './registration-form.component.html',
  styleUrls: ['./registration-form.component.css']
})
export class RegistrationFormComponent implements OnInit {
  errors: string = "";
  isRequesting: boolean = false;
  submitted: boolean = false;

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit() {
  }

  registerUser({ value }: { value: UserRegistration}) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors='';
    if(true)
    {
      this.userService.register(value)
        .subscribe(
          result  => {if(result){
            this.router.navigate(['account/login']);
          }},
          errors =>  this.errors = errors);
    }
  }



}
