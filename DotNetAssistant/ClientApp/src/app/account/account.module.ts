import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrationFormComponent } from './registration-form/registration-form.component';
import { LoginFormComponent } from './login-form/login-form.component';
import {AccountRouting} from "./account.routing";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { FavoriteColorComponent } from './favorite-color/favorite-color.component';

@NgModule({
  declarations: [
    RegistrationFormComponent,
    LoginFormComponent,
    FavoriteColorComponent
  ],
    imports: [
        CommonModule,
        AccountRouting,
        FormsModule,
        ReactiveFormsModule
    ]
})
export class AccountModule { }
