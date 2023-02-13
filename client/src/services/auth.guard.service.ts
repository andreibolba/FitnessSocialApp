import { Injectable, OnDestroy, OnInit } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    const personString = localStorage.getItem('person');
    if (!personString) {
      this.router.navigate(['']);
      return false;
    } else {
      return true;
    }
  }

}
