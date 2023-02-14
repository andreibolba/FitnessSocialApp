import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from './header/header.component';
import { AuthentificationComponent } from './authentification/authentification.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminDashboardComponent } from './admin/admin.dashboard/admin.dashboard.component';
import { TrainerDashboardComponent } from './trainer/trainer.dashboard/trainer.dashboard.component';
import { InternDashboardComponent } from './intern/intern.dashboard/intern.dashboard.component';
import { ForumComponent } from './forum/forum/forum.component';


@NgModule({
  declarations: [AppComponent, HeaderComponent, AuthentificationComponent, HomeComponent, AdminDashboardComponent, TrainerDashboardComponent, InternDashboardComponent, ForumComponent],
  imports: [BrowserModule, HttpClientModule, FormsModule,AppRoutingModule, BrowserAnimationsModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
