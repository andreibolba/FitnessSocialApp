import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map, Subject, Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { PresenceService } from './presence.service';
import { DataStorageService } from './data-storage.service';
import { LogService } from './log.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = 'https://localhost:7191/api/';
  personSubscription!: Subscription;
  private currentPersonSource = new BehaviorSubject<LoggedPerson | null>(null);
  currentPerson$ = this.currentPersonSource.asObservable();

  constructor(private http: HttpClient,private dataStorage:DataStorageService, private router: Router, private presenceService:PresenceService, private log:LogService) {}

  login(model: any) {
    
    return this.http
      .post<LoggedPerson>(this.baseUrl + 'account/login', model)
      .pipe(
        map((respose: LoggedPerson) => {
          const person = respose;
          if (person) {
            localStorage.setItem('person', JSON.stringify(person));
            this.currentPersonSource.next(person);
            this.setCurentPerson(person);
          }
        })
      );
  }

  setCurentPerson(person: LoggedPerson) {
    this.currentPersonSource.next(person);
    this.personSubscription = this.dataStorage.personData
        .getPerson(person.username, person.token)
        .subscribe((res) => {
          this.presenceService.createHubConnection(res, person.token);
        });
  }

  logout() {
    if (this.router.url.includes('recovery')==false) this.router.navigate(['']);
    localStorage.removeItem('person');
    this.presenceService.stopHubConnection();
  }
}
