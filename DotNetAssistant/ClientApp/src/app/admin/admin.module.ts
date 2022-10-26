import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AdminRouting} from "./admin.routing";
import { EditQuestionComponent } from './question/edit-question/edit-question.component';
import {ReactiveFormsModule} from "@angular/forms";


@NgModule({
  declarations: [

    EditQuestionComponent
  ],
  imports: [
    CommonModule,
    AdminRouting,
    ReactiveFormsModule
  ]
})
export class AdminModule { }
