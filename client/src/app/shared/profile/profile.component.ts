import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DashboardService } from 'src/services/dashboard.service';
import { DataStorageService } from 'src/services/data-storage.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit, OnDestroy {
  dataSub!: Subscription;
  person!: Person;
  isLoading=true;

  constructor(private dataService: DataStorageService, private dashServ:DashboardService) {}
  ngOnInit(): void {
    this.isLoading=true;
    this.dashServ.dashboardChanged.emit(false);
    console.log('perosnal');
    this.setCurrentUser();
  }
  ngOnDestroy(): void {
    if(this.dataSub) this.dataSub.unsubscribe();
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
          ()=>{
            this.isLoading=false;
          }
        );
    }
  }
}
