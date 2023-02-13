import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { BehaviorSubject, map, Subject } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = 'https://localhost:7191/api/';
  private currentPersonSource = new BehaviorSubject<LoggedPerson | null>(null);
  currentPerson$ = this.currentPersonSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http
      .post<LoggedPerson>(this.baseUrl + 'account/login', model)
      .pipe(
        map((respose: LoggedPerson) => {
          const person = respose;
          if (person) {
            localStorage.setItem('person', JSON.stringify(person));
            this.currentPersonSource.next(person);
          }
        })
      );
  }

  setCurentPerson(person: LoggedPerson) {
    this.currentPersonSource.next(person);
  }

  logout() {
    localStorage.removeItem('person');
  }
}
