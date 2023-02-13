import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  authChanged = new BehaviorSubject<boolean>(true);
  baseUrl = 'https://localhost:7191/api/';
  private currectPersonSource = new BehaviorSubject<LoggedPerson | null>(null);
  currentPerson$ = this.currectPersonSource.asObservable();

  constructor(private http: HttpClient) {
    this.authChanged.next(true);
  }

  login(model: any) {
    return this.http
      .post<LoggedPerson>(this.baseUrl + 'account/login', model)
      .pipe(
        map((respose: LoggedPerson) => {
          const person = respose;
          if (person) {
            localStorage.setItem('person', JSON.stringify(person));
            this.currectPersonSource.next(person);
          }
        })
      );
  }

  setCurerentPerson(person: LoggedPerson) {
    this.currectPersonSource.next(person);
  }

  logout() {
    localStorage.removeItem('person');
    this.currectPersonSource.next(null);
  }
}
