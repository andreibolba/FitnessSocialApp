import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Day } from 'src/model/day.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Meeting } from 'src/model/meeting.model';
import { Person } from 'src/model/person.model';
import { CalendarCreatorService } from 'src/services/calendar.creator.service';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-trainer-dashboard',
  templateUrl: './trainer.dashboard.component.html',
  styleUrls: ['./trainer.dashboard.component.css']
})
export class TrainerDashboardComponent {
  meetingsLink = 'meetings';
  monthDays!: Day[];
  monthNumber!: number;
  year!: number;
  weekDaysName: string[] = [];

  dataSub!: Subscription;
  meetingSub!: Subscription;

  person!: Person;
  meetings:Meeting[]=[];
  isLoading=true;

  constructor(
    public calendarCreator: CalendarCreatorService,
    private utils: UtilsService,
    private dataService: DataStorageService,
    private router:Router
  ) {}

  ngOnDestroy(): void {
    if(this.dataSub) this.dataSub.unsubscribe();
    if(this.meetingSub) this.meetingSub.unsubscribe();
  }

  ngOnInit(): void {
    this.utils.initializeError();
    if(this.router.url!='/dashboard')
    this.utils.dashboardChanged.next(false);
    this.isLoading=true;
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
    this.utils.dashboardChanged.next(false);
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
            this.meetingSub=this.dataService.getANumberOfMeetingsForPerson(person.token,res.personId,res.status.toLocaleLowerCase(),3).subscribe((data)=>{
              this.meetings=data;
            },(error) => {
              console.log(error.error);
            },
            ()=>{
              this.isLoading=false;
            })
          },
          (error) => {
            console.log(error.error);
          }
        );
    }
  }
}
