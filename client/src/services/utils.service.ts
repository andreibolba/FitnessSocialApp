import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Group } from 'src/model/group.model';
import { Person } from 'src/model/person.model';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {

  userToEdit=new BehaviorSubject<Person | null>(null);
  groupToEdit=new BehaviorSubject<Group | null>(null);

  constructor() { }

  static initialize(){

  }
}
