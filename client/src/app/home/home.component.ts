import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DashboardService } from 'src/services/dashboard.service';
import { DataStorageService } from 'src/services/data-storage.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit, OnDestroy {
  title = 'InternShip App';
  people: any;
  person!: Person;
  authSub!: Subscription;
  dataSub!: Subscription;
  dashSub!:Subscription;
  isDashboard=true;
  isLoading=false;
  profile='profile';

  constructor(
    private dataService: DataStorageService,
    private dashService:DashboardService
  ) {}

  ngOnDestroy(): void {}

  ngOnInit(): void {
    this.dashSub=this.dashService.dashboardChanged.subscribe(res=>{
      this.isDashboard=res;
    });
    this.isLoading=true;
    this.setCurrentUser();
    this.person=new Person();
    console.log(this.isDashboard);
  }

  setCurrentUser() {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.dataSub = this.dataService
        .getPerson(person.username,person.token)
        .subscribe(
          (res) => {this.person=res},
          (error) => {
            console.log(error.error);
          },
          ()=>{
            this.isLoading=false;
          }
        );
    }
  }

  onProfile() {
    this.dashService.dashboardChanged.emit(false);
  }
}
