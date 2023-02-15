import { Component, ViewChild } from '@angular/core';
import { MatCalendar } from '@angular/material/datepicker';
import { Day } from 'src/model/day.model';
import { CalendarCreatorService } from 'src/services/calendar.creator.service';
import { DashboardService } from 'src/services/dashboard.service';

@Component({
  selector: 'app-intern-dashboard',
  templateUrl: './intern.dashboard.component.html',
  styleUrls: ['./intern.dashboard.component.css']
})
export class InternDashboardComponent {
  feedback=[
    {'feedback': 'Very good job! Keep up with the good work!', 'date':' 15-Feb-2023'},
    {'feedback': 'Very good job! Keep up with the good work!Very good job! Keep up with the good work!', 'date':' 15-Feb-2023'},
    {'feedback': 'Very good job!', 'date':' 15-Feb-2023'},
  ];
  hasFeedback=true;
  feedbackLink='feedback';

  meetings=[
    {'name': 'Semestrial Meeting with the team', 'date':' 16-Feb-2023 5:00 PM', 'link': 'meetings'},
    {'name': 'Meeting with the mentor', 'date':' 17-Feb-2023 10:00 AM', 'link': 'meetings'},
    {'name': 'Meeting with the client', 'date':' 19-Feb-2023 1:15 PM', 'link': 'meetings'},
  ];
  hasMeetings=true;
  meetingsLink='meetings';

  public monthDays!: Day[];

  public monthNumber!: number;
  public year!: number;

  public weekDaysName:string[] = [];

  constructor(public calendarCreator: CalendarCreatorService, private dashServ:DashboardService) {}

  ngOnInit(): void {
    this.setMonthDays(this.calendarCreator.getCurrentMonth());

    this.weekDaysName.push("Mo");
    this.weekDaysName.push("Tu");
    this.weekDaysName.push("We");
    this.weekDaysName.push("Th");
    this.weekDaysName.push("Fr");
    this.weekDaysName.push("Sa");
    this.weekDaysName.push("Su");
  }

  onNextMonth(): void {
    this.monthNumber++;

    if (this.monthNumber == 13) {
      this.monthNumber = 1;
      this.year++;
    }

    this.setMonthDays(this.calendarCreator.getMonth(this.monthNumber, this.year));
  }

  onPreviousMonth() : void{
    this.monthNumber--;

    if (this.monthNumber < 1) {
      this.monthNumber = 12;
      this.year--;
    }

    this.setMonthDays(this.calendarCreator.getMonth(this.monthNumber, this.year));
  }

  private setMonthDays(days: Day[]): void {
    this.monthDays = days;
    this.monthNumber = this.monthDays[0].monthIndex;
    this.year = this.monthDays[0].year;
  }

  onLeaveDashboardClick(){
    this.dashServ.dashboardChanged.emit(false);
  }
}
