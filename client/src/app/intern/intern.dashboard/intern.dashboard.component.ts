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
    {'feedback': 'Very good job! Keep up with the good work', 'date':' 15-Feb-2023'},
    {'feedback': 'Very good job! Keep up with the good work!Very good job! Keep up with the good work!', 'date':' 15-Feb-2023'},
    {'feedback': 'Very good job! Keep up with the good work', 'date':' 15-Feb-2023'},
  ];
  hasFeedback=true;
  feedbackLink='feedback';

  constructor(private dashServ:DashboardService){}

  onfeedbackclick(){
    this.dashServ.dashboardChanged.emit(false);
  }
}
