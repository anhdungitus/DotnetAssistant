import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import  {AppRoutingModule } from "./app.routing.module";
import { HomeComponent } from './home/home.component';
import {AuthGuard} from "./auth.guard";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatTableModule} from "@angular/material/table";
import {QuestionComponent} from "./admin/question/question.component";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from "@angular/material/button";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatSortModule} from "@angular/material/sort";
import {PlanComponent} from "./plan/plan.component";

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    QuestionComponent,
    PlanComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    MatPaginatorModule,
    MatTableModule,
    BrowserAnimationsModule,
    MatDialogModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSortModule
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
