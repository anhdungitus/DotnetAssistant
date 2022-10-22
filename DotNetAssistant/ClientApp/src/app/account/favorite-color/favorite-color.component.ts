import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormControl, Validators} from '@angular/forms';
import {UserService} from "../../shared/services/user.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-favorite-color',
  templateUrl: './favorite-color.component.html',
  styleUrls: ['./favorite-color.component.css']
})
export class FavoriteColorComponent implements OnInit {
  errors: string = "";
  registerForm = this.fb.group({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    firstName: new FormControl('', [Validators.required]),
    lastName: '',
    location: ''
  });
  constructor(private fb: FormBuilder, private userService: UserService, private router: Router) { }

  get email() { return this.registerForm.get('email')!; }

  ngOnInit(): void {
  }

  onSubmit() {
    this.userService.register(this.registerForm.value)
      .subscribe(
        result  => {if(result){
          this.router.navigate(['account/login']).then(r => r);
        }},
        errors =>  this.errors = errors);
  }
}
