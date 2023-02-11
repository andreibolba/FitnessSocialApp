import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { AuthService } from 'src/services/auth.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit,OnDestroy{
  sub!:Subscription;
  loggedPersonUsername!: string | undefined;

  constructor(private authSer:AuthService) {

  }
  ngOnInit(): void {
    this.sub=this.authSer.authChanged.subscribe();
    this.setCurrentUser();
  }

  ngOnDestroy(): void {
    if(this.sub)
      this.sub.unsubscribe();
  }

  register(){
    this.authSer.authChanged.next(false);
  }

  onLogOut(){
    this.authSer.logout();
  }

  setCurrentUser() {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.loggedPersonUsername = person.username;
      this.authSer.setCurerentPerson(person);
    }
  }
}
