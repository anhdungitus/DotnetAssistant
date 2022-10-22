import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import  {AppRoutingModule } from "./app.routing.module";
import { HomeComponent } from './home/home.component';
import {AuthGuard} from "./auth.guard";


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule

],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
