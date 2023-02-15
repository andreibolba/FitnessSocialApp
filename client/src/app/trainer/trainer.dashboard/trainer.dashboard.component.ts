import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { Day } from 'src/model/day.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { CalendarCreatorService } from 'src/services/calendar.creator.service';
import { DashboardService } from 'src/services/dashboard.service';
import { DataStorageService } from 'src/services/data-storage.service';

@Component({
  selector: 'app-trainer-dashboard',
  templateUrl: './trainer.dashboard.component.html',
  styleUrls: ['./trainer.dashboard.component.css']
})
export class TrainerDashboardComponent {
  meetings = [
    {
      name: 'Semestrial Meeting with the team',
      date: ' 16-Feb-2023 5:00 PM',
      link: 'meetings',
    },
    {
      name: 'Meeting with the mentor',
      date: ' 17-Feb-2023 10:00 AM',
      link: 'meetings',
    },
    {
      name: 'Meeting with the client',
      date: ' 19-Feb-2023 1:15 PM',
      link: 'meetings',
    },
  ];
  hasMeetings = true;
  meetingsLink = 'meetings';
  monthDays!: Day[];
  monthNumber!: number;
  year!: number;
  weekDaysName: string[] = [];
  dataSub!: Subscription;
  person!: Person;

  constructor(
    public calendarCreator: CalendarCreatorService,
    private dashServ: DashboardService,
    private dataService: DataStorageService
  ) {}

  ngOnDestroy(): void {
    if(this.dataSub) this.dataSub.unsubscribe();
  }

  ngOnInit(): void {
    this.setMonthDays(this.calendarCreator.getCurrentMonth());

    this.weekDaysName.push('Mo');
    this.weekDaysName.push('Tu');
    this.weekDaysName.push('We');
    this.weekDaysName.push('Th');
    this.weekDaysName.push('Fr');
    this.weekDaysName.push('Sa');
    this.weekDaysName.push('Su');

    this.setCurrentUser();
  }

  private setMonthDays(days: Day[]): void {
    this.monthDays = days;
    this.monthNumber = this.monthDays[0].monthIndex;
    this.year = this.monthDays[0].year;
  }

  onLeaveDashboardClick() {
    this.dashServ.dashboardChanged.emit(false);
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
          }
        );
    }
  }
}
