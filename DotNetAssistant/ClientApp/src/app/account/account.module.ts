import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrationFormComponent } from './registration-form/registration-form.component';
import { LoginFormComponent } from './login-form/login-form.component';
import {AccountRouting} from "./account.routing";
import {FormsModule} from "@angular/forms";

@NgModule({
  declarations: [
    RegistrationFormComponent,
    LoginFormComponent
  ],
    imports: [
        CommonModule,
        AccountRouting,
        FormsModule
    ]
})
export class AccountModule { }
