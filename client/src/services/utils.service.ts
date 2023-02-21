import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Person } from 'src/model/person.model';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {

  userToEdit=new BehaviorSubject<Person | null>(null);

  constructor() { }

  static initialize(){
    
  }
}
