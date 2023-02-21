import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Group } from 'src/model/group.model';
import { Person } from 'src/model/person.model';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {

  userToEdit=new BehaviorSubject<Person | null>(null);
  groupToEdit=new BehaviorSubject<Group | null>(null);
  dashboardChanged=new BehaviorSubject<boolean>(true);

  constructor() { }

  static initialize(){

  }
}
