import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit, OnDestroy {
  dataSub!: Subscription;
  person!: Person;
  isLoading=true;

  constructor(private dataService: DataStorageService, private utils:UtilsService) {}
  ngOnInit(): void {
    this.isLoading=true;
    this.utils.dashboardChanged.next(false);
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
