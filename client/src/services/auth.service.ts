import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService{
  authChanged = new BehaviorSubject<boolean>(true);
  constructor() {
    this.authChanged.next(true);
  }
}
