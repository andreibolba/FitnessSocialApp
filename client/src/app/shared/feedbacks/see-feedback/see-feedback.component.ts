import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Feedback } from 'src/model/feedback.model';
import { Group } from 'src/model/group.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-see-feedback',
  templateUrl: './see-feedback.component.html',
  styleUrls: ['./see-feedback.component.css']
})
export class SeeFeedbackComponent implements OnInit, OnDestroy {
  getFeedbakSubscription!: Subscription;
  addEditSubscription!: Subscription;
  getPeopleSubscription!: Subscription;
  getTasksSubscription!: Subscription;
  getTestsSubscription!: Subscription;
  getChallengesSubscription!: Subscription;
  feedback:Feedback=new Feedback();
  fullName:string='';
  private token: string = '';

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService
  ) {
  }

  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.getFeedbakSubscription = this.utils.feedbackToEdit.subscribe((res) => {
        if(res)
          this.feedback=res;
          this.fullName = res?.personSender.firstName + ' '+res?.personSender.lastName;
      });

    }
  }

  ngOnDestroy(): void {
    if (this.getFeedbakSubscription != null) this.getFeedbakSubscription.unsubscribe();
    if (this.addEditSubscription != null) this.addEditSubscription.unsubscribe();
  }

}

