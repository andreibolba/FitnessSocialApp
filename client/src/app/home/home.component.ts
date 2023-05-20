import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit, OnDestroy {
  person!: Person;
  authSub!: Subscription;
  dataSub!: Subscription;
  utilsSub!: Subscription;
  isDashboard = true;
  isLoading = false;
  profile = 'profile';

  constructor(
    private dataService: DataStorageService,
    private utils: UtilsService
  ) {}

  ngOnDestroy(): void {
    if (this.authSub) this.authSub.unsubscribe();
    if (this.dataSub) this.dataSub.unsubscribe();
    if (this.utilsSub) this.utilsSub.unsubscribe();
  }

  ngOnInit(): void {
    this.utils.initializeError();
    this.utilsSub = this.utils.dashboardChanged.subscribe((res) => {
      this.isDashboard = res;
    });

    this.isLoading = true;
    this.setCurrentUser();
    this.person = new Person();
  }

  setCurrentUser() {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.dataSub = this.dataService
        .getPerson(person.username, person.token)
        .subscribe(
          (res) => {
            this.person = res;
          },
          (error) => {
            console.log(error.error);
          },
          () => {
            this.isLoading = false;
          }
        );
    }
  }

  onProfile() {
    this.utils.dashboardChanged.next(false);
  }

  onChatClick(){
    this.utils.dashboardChanged.next(false);
  }
}
