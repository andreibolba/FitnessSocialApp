import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { AuthService } from 'src/services/auth.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit{
  loggedPersonUsername!: string | undefined;

  constructor(private authSer:AuthService,private router:Router) {

  }
  ngOnInit(): void {
    this.setCurrentUser();
  }

  onLogOut(){
    this.router.navigate(['']);
    this.authSer.logout();
  }

  setCurrentUser() {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.loggedPersonUsername = person.username;
    }
  }
}
