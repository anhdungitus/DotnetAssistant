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



@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    QuestionComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    MatPaginatorModule,
    MatTableModule,
    BrowserAnimationsModule,
    MatDialogModule,
    ReactiveFormsModule
],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
