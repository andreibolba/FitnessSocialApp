import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { AuthService } from 'src/services/auth.service';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

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
    private dataService: DataStorageService,
    private utils:UtilsService
  ) {}
  ngOnDestroy(): void {
    this.dataSub.unsubscribe();
  }

  ngOnInit(): void {
    this.utils.initializeError();
    this.setCurrentUser();
  }

  onLogOut() {
    this.authSer.logout();
  }

  onLeaveDashBoard(name: string) {
    if (name == 'Dashboard') this.utils.dashboardChanged.next(true);
    else this.utils.dashboardChanged.next(false);
    if(name == 'Mettings' || name=='Tests') {this.utils.isFromGroupDashboard.next(false);}
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
                  { name: 'Notes', logo: 'uil uil-notes', link: 'notes' },
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
                  { name: 'Dashboard', logo: 'uil uil-estate', link: '../dashboard' },
                  { name: 'My Group', logo: 'uil uil-users-alt', link: 'mygroups' },
                  { name: 'Tests', logo: 'uil uil-question-circle', link: 'tests' },
                  { name: 'Questions', logo: 'uil uil-font', link: 'questions' },
                  { name: 'Challanges', logo: 'uil uil-brackets-curly',link: 'challanges' },
                  { name: 'Feedback', logo: 'uil uil-feedback', link: 'feedback' },
                  { name: 'Notes', logo: 'uil uil-notes', link: 'notes' },
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
              case 'Intern':
                this.buttons = [
                  { name: 'Dashboard', logo: 'uil uil-estate', link: '../dashboard' },
                  { name: 'My Groups', logo: 'uil uil-users-alt', link: 'mygroups' },
                  { name: 'Tests', logo: 'uil uil-diary', link: 'tests' },
                  { name: 'Challanges', logo: 'uil uil-brackets-curly',link: 'challanges' },
                  { name: 'Feedback', logo: 'uil uil-feedback', link: 'feedback' },
                  { name: 'Notes', logo: 'uil uil-notes', link: 'notes' },
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
              default:
                break;
            }
          }
        );
    }
  }
}
