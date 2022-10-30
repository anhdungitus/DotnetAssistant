import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AdminRouting} from "./admin.routing";
import { EditQuestionComponent } from './question/edit-question/edit-question.component';
import {ReactiveFormsModule} from "@angular/forms";
import { AddQuestionComponent } from './question/add-question/add-question.component';
import { ConfirmationDialogComponent } from './question/confirmation-dialog/confirmation-dialog.component';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';


@NgModule({
  declarations: [

    EditQuestionComponent,
     AddQuestionComponent,
     ConfirmationDialogComponent,
     AdminLayoutComponent
  ],
  imports: [
    CommonModule,
    AdminRouting,
    ReactiveFormsModule
  ]
})
export class AdminModule { }
