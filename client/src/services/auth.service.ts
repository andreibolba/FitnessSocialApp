import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  displayChanged = new EventEmitter<boolean>(true);
  constructor() { }
}
