import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AdminRouting} from "./admin.routing";
import { EditQuestionComponent } from './question/edit-question/edit-question.component';
import {ReactiveFormsModule} from "@angular/forms";
import { AddQuestionComponent } from './question/add-question/add-question.component';


@NgModule({
  declarations: [

    EditQuestionComponent,
     AddQuestionComponent
  ],
  imports: [
    CommonModule,
    AdminRouting,
    ReactiveFormsModule
  ]
})
export class AdminModule { }
