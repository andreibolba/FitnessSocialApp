import { Injectable } from "@angular/core";
import { LoggedPerson } from "src/model/loggedperson.model";
import { AuthService } from "src/services/auth.service";
@Injectable({
  providedIn: 'root',
})
export class Utils{

  constructor(private authSer:AuthService){}

  setCurrentUser():LoggedPerson | null{
    console.log('utils');
    const personString = localStorage.getItem('person');
    if(!personString) return null;
    const person:LoggedPerson = JSON.parse(personString);
    this.authSer.setCurerentPerson(person);
    return person;
  }
}
