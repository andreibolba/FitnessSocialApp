import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { SeeMeetingParticipantsComponent } from 'src/app/shared/meetings/see-meeting-participants/see-meeting-participants.component';
import { Day } from 'src/model/day.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Meeting } from 'src/model/meeting.model';
import { Person } from 'src/model/person.model';
import { CalendarCreatorService } from 'src/services/calendar.creator.service';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-intern-dashboard',
  templateUrl: './intern.dashboard.component.html',
  styleUrls: ['./intern.dashboard.component.css'],
})
export class InternDashboardComponent implements OnInit, OnDestroy {
  feedback = [
    {
      feedback: 'Very good job! Keep up with the good work!',
      date: ' 15-Feb-2023',
    },
    {
      feedback:
        'Very good job! Keep up with the good work!Very good job! Keep up with the good work!',
      date: ' 15-Feb-2023',
    },
    { feedback: 'Very good job!', date: ' 15-Feb-2023' },
  ];
  hasFeedback = true;
  feedbackLink = 'feedback';
  meetings:Meeting[]=[];
  meetingsLink = 'meetings';
  participants:string=" and other ";
  monthDays!: Day[];
  monthNumber!: number;
  year!: number;
  weekDaysName: string[] = [];
  dataSub!: Subscription;
  meetingSub!: Subscription;
  person!: Person;
  isLoading=true;

  constructor(
    public calendarCreator: CalendarCreatorService,
    private utils: UtilsService,
    private dataService: DataStorageService,
    private router:Router,
    private dialog: MatDialog
  ) {}

  ngOnDestroy(): void {
    if(this.dataSub) this.dataSub.unsubscribe();
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
      this.dataSub = this.dataService.personData
        .getPerson(person.username, person.token)
        .subscribe(
          (res) => {
            this.person = res;
            this.meetingSub=this.dataService.getANumberOfMeetingsForPerson(person.token,res.personId,res.status.toLocaleLowerCase(),3).subscribe((data)=>{
              this.meetings=data;
              this.meetings.forEach(element => {
                element.participants='';
                if(element.allPeopleInMeeting.length==0)
                  element.participants='No participants';
                else if(element.allPeopleInMeeting.length>=3){
                  element.participants+=element.allPeopleInMeeting[0].firstName+" "+element.allPeopleInMeeting[0].lastName+", ";
                  element.participants+=element.allPeopleInMeeting[1].firstName+" "+element.allPeopleInMeeting[1].lastName;
                }else{
                  element.allPeopleInMeeting.forEach(person => {
                    element.participants+=person.firstName+" "+person.lastName+", ";
                  });
                }
              });
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

  openDialog() {
    const dialogRef = this.dialog.open(SeeMeetingParticipantsComponent);

    dialogRef.afterClosed().subscribe((result) => {
      console.log(`Dialog result: ${result}`);
    });
  }

  seeMeeting(people:Person[]){
    this.utils.meetingParticipants.next(people);
    this.openDialog();
  }
}
