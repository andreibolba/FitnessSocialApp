import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { AuthService } from 'src/services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit, OnDestroy {
  title = 'InternShip App';
  people: any;
  person=new Person();
  authSub!: Subscription;
  peopleSub!: Subscription;
  loggedPersonusername!: string | undefined;
  personFirstname!:string|null;

  constructor(
    private http: HttpClient,
    private authSer: AuthService,
    private router: Router
  ) {}
  ngOnDestroy(): void {
    if (this.authSub) this.authSub.unsubscribe();
  }

  ngOnInit(): void {
    this.getPeople();
    this.setCurrentUser();
  }

  getPeople() {
    this.http.get('https://localhost:7191/api/people').subscribe({
      next: (response) => {
        this.people=response;
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log('Request has finished');
      },
    });
  }

  setCurrentUser() {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.loggedPersonusername = person.username;
      this.authSer.setCurerentPerson(person);
      this.http.get<Person>("https://localhost:7191/api/people/"+this.loggedPersonusername).subscribe({
      next: (response) => {
        this.person=response;
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {
        console.log('Request has finished');
      },
    });
    }
  }

  onProfile() {
    console.log(this.person.firstName);
  }
}
