import { EventEmitter, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { Group } from 'src/model/group.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from './data-storage.service';

@Injectable({
  providedIn: 'root'
})
export class UtilsService {

  userToEdit=new BehaviorSubject<Person | null>(null);
  groupToEdit=new BehaviorSubject<Group | null>(null);
  addedPerson=new BehaviorSubject<Person | null>(null);
  dashboardChanged=new EventEmitter<boolean>(true);

  error=new BehaviorSubject<{errorCode:number,errorTitle:string,errorMessage:string} | null>(null);

  constructor(private data:DataStorageService,private router:Router) { }

  initializeError(){
   this.data.testConnection().subscribe(()=>{
   },()=>{
      this.error.next({
        errorCode: 500,
        errorTitle: 'Internal Server Error!',
        errorMessage:' An error has occured! We apologise and we are fixing the problem! Please try again later!'
      });
      this.router.navigate(["error"]);
   })
  }
}
