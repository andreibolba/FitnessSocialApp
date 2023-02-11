import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/services/auth.guard.service';
import { AuthentificationComponent } from './authentification/authentification.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: '', component:AuthentificationComponent},
  { path: 'authentification', component: AuthentificationComponent },
  { path: 'dashboard', component:HomeComponent,canActivate: [AuthGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
