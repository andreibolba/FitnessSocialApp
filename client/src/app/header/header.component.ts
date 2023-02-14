import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { AuthService } from 'src/services/auth.service';
import { DashboardService } from 'src/services/dashboard.service';
import { DataStorageService } from 'src/services/data-storage.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit, OnDestroy {
  loggedPersonUsername!: string | undefined;
  person!: Person;
  dataSub!: Subscription;
  buttons: any;
  constructor(
    private authSer: AuthService,
    private router: Router,
    private dataService: DataStorageService,
    private dashService: DashboardService
  ) {}
  ngOnDestroy(): void {
    this.dataSub.unsubscribe();
  }

  ngOnInit(): void {
    this.setCurrentUser();
  }

  onLogOut() {
    this.router.navigate(['']);
    this.authSer.logout();
  }

  onLeaveDashBoard(name: string) {
    if (name == 'Dashboard') this.dashService.dashboardChanged.emit(true);
    else this.dashService.dashboardChanged.emit(false);
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
            switch (this.person.status) {
              case 'Admin':
                this.buttons = [
                  {
                    name: 'Dashboard',
                    logo: 'uil uil-estate',
                    link: '../dashboard',
                  },
                  {
                    name: 'Admin',
                    logo: 'uil uil-user-md',
                    link: 'administrators',
                  },
                  { name: 'Trainer', logo: 'uil uil-user', link: 'trainers' },
                  {
                    name: 'Intern',
                    logo: 'uil uil-book-reader',
                    link: 'interns',
                  },
                  { name: 'Group', logo: 'uil uil-users-alt', link: 'groups' },
                  {
                    name: 'Mettings',
                    logo: 'uil uil-meeting-board',
                    link: 'meetings',
                  },
                  {
                    name: 'Tasks',
                    logo: 'uil uil-clipboard-notes',
                    link: 'tasks',
                  },
                  { name: 'Forum', logo: 'uil uil-font', link: 'forum' },
                ];
                break;
              case 'Trainer':
                this.buttons = [
                  { name: 'Dashboard Tr', logo: 'uil uil-estate' },
                  { name: 'Dashboard Tr', logo: 'uil uil-estate' },
                  { name: 'Dashboard Tr', logo: 'uil uil-estate' },
                ];
                break;
              case 'Intern':
                this.buttons = [
                  { name: 'Dashboard Tr', logo: 'uil uil-estate' },
                  { name: 'Dashboard Tr', logo: 'uil uil-estate' },
                  { name: 'Dashboard Tr', logo: 'uil uil-estate' },
                ];
                break;
              default:
                break;
            }
          }
        );
    }
  }
}
