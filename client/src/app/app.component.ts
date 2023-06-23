import { HttpClient } from '@angular/common/http';
import { Component, HostListener, OnInit } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { PresenceService } from 'src/services/presence.service';
import { UtilsService } from 'src/services/utils.service';

export let browserRefresh = false;
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  subscription!: Subscription;
  getCurrentPersonSubscription!: Subscription;

  constructor(private router: Router, private utils: UtilsService, private presenceService: PresenceService, private dataStorage: DataStorageService) {
    this.subscription = router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        browserRefresh = !router.navigated; 
        const personString = localStorage.getItem('person');
        if (!personString) {
          return;
        } else {
          const person: LoggedPerson = JSON.parse(personString);
          this.getCurrentPersonSubscription = this.dataStorage
            .getPerson(person.username, person.token)
            .subscribe((data) => {
              this.presenceService.createHubConnection(data, person.token);
            });
        }
      }
    });
  }

  ngOnInit(): void {
  }

}
