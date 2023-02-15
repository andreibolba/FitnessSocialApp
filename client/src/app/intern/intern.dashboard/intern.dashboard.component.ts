import { Component, ViewChild } from '@angular/core';
import { MatCalendar } from '@angular/material/datepicker';
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

  constructor(private dashServ:DashboardService){}

  onLeaveDashboardClick(){
    this.dashServ.dashboardChanged.emit(false);
  }
}
