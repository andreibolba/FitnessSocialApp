import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Group } from 'src/model/group.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DashboardService } from 'src/services/dashboard.service';
import { DataStorageService } from 'src/services/data-storage.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin.dashboard.component.html',
  styleUrls: ['./admin.dashboard.component.css'],
})
export class AdminDashboardComponent implements OnInit, OnDestroy {
  dataPeopleSub!: Subscription;
  dataGroupsSub!: Subscription;
  groupsNumber: number = 0;
  trainersNumber: number = 0;
  internsNumber: number = 0;
  person!:Person;
  isLoading=true;

  constructor(
    private http: HttpClient,
    private dashServ: DashboardService,
    private dataService: DataStorageService,
    private router:Router
  ) {}

  ngOnDestroy(): void {
    if (this.dataPeopleSub) this.dataPeopleSub.unsubscribe();
    if (this.dataGroupsSub) this.dataGroupsSub.unsubscribe();
  }

  ngOnInit(): void {
    if(this.router.url!='/dashboard')
    this.dashServ.dashboardChanged.emit(false);
    this.isLoading=true;
    this.setCurrentUser();
  }

  setCurrentUser() {
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.dataPeopleSub = this.dataService.getPeople(person.token).subscribe(
        (res) => {
          let people: Person[] = res;
          this.internsNumber = people.filter(p=>p.status=='Intern').length;
          this.trainersNumber = people.filter(p=>p.status=='Trainer').length;
          this.person=people.find(p=>p.username==person.username) as Person;

          this.dataService.people=res;
        },
        (error) => {
          console.log(error.error);
        }
      );
      this.dataGroupsSub = this.dataService.getGroups(person.token).subscribe(
        (res) => {
          let groups: Group[] = res;
          this.groupsNumber = groups.length;
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
