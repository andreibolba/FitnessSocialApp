import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Note } from 'src/model/note.model';
import { Person } from 'src/model/person.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';
import { AddEditNoteComponent } from '../../notes/add-edit-note/add-edit-note.component';
import { Feedback } from 'src/model/feedback.model';

export class PersonCombo {
  public PersonId: number;
  public PersonName: string;

  constructor() {
    this.PersonId = -1;
    this.PersonName = '';
  }
}

export class TaskCombo {
  public TaskId: number;
  public TaskName: string;

  constructor() {
    this.TaskId = -1;
    this.TaskName = '';
  }
}

export class TestCombo {
  public TestId: number;
  public TestName: string;

  constructor() {
    this.TestId = -1;
    this.TestName = '';
  }
}

export class ChallengeCombo {
  public ChallengeId: number;
  public ChallengeName: string;

  constructor() {
    this.ChallengeId = -1;
    this.ChallengeName = '';
  }
}

@Component({
  selector: 'app-add-edit-feedback',
  templateUrl: './add-edit-feedback.component.html',
  styleUrls: ['./add-edit-feedback.component.css']
})
export class AddEditFeedbackComponent implements OnInit, OnDestroy {
  getFeedbakSubscription!: Subscription;
  getPersonSubscription!: Subscription;
  addEditSubscription!: Subscription;
  getPeopleSubscription!: Subscription;
  getTasksSubscription!: Subscription;
  getTestsSubscription!: Subscription;
  getChallengesSubscription!: Subscription;
  content: string = '';
  grade: number = 1;
  operation: string = '';
  person: Person = new Person();
  people: PersonCombo[] = [];
  tasks: TaskCombo[] = [];
  challenges: ChallengeCombo[] = [];
  tests: TestCombo[] = [];
  private token: string = '';
  private feedbackId: number = -1;
  private currentPersonId: number = -1;
  private currentTestId: number = -1;
  private currentTaskId: number = -1;
  private currentChallengeId: number = -1;

  constructor(
    private utils: UtilsService,
    private dataStorage: DataStorageService,
    private dialog: MatDialogRef<AddEditNoteComponent>,
    private toastr: ToastrService
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
        if (res) {
          this.operation = "Edit";
          this.content = res.content;
          this.feedbackId = res.feedbackId;
          this.grade = res.grade;
        } else {
          this.operation = "Add";
          this.content = "";
          this.grade = 1;
        }

        this.getPersonSubscription = this.dataStorage.getPerson(person.username, this.token).subscribe((data) => {
          if (data) {
            this.person = data;
            this.getPeopleSubscription = this.dataStorage.getPeople(this.token).subscribe((ppl) => {
              ppl.forEach(element => {
                let ic = new PersonCombo;
                ic.PersonId = element.personId;
                ic.PersonName = element.firstName + ' ' + element.lastName;
                this.people.push(ic);
              });
              this.currentPersonId = ppl[0].personId;
            }, () => { }, () => {
              if (res != null) {
                let indexPerson = this.people.findIndex(g => g.PersonId == res.personReceiverId);
                if (indexPerson != -1) {
                  [this.people[0], this.people[indexPerson]] = [this.people[indexPerson], this.people[0]];
                }
              }
            });

            this.getChallengesSubscription = this.dataStorage.getAllChallenges(this.token).subscribe((ppl) => {
              this.challenges.push(new ChallengeCombo());
              ppl.forEach(element => {
                let ic = new ChallengeCombo;
                ic.ChallengeId = element.challangeId;
                ic.ChallengeName = element.challangeName;
                this.challenges.push(ic);
              });
            }, () => { }, () => {
              if (res != null) {
                let indexChallenge = this.challenges.findIndex(g => g.ChallengeId == res.challangeId);
                if (indexChallenge != -1) {
                  [this.challenges[0], this.challenges[indexChallenge]] = [this.challenges[indexChallenge], this.challenges[0]];
                }
              }
            });

            this.getTestsSubscription = this.dataStorage.getAllTests(this.token).subscribe((ppl) => {
              this.tests.push(new TestCombo());
              ppl.forEach(element => {
                let ic = new TestCombo;
                ic.TestId = element.testId;
                ic.TestName = element.testName;
                this.tests.push(ic);
              });
            }, () => { }, () => {
              if (res != null) {
                let indexTest = this.tests.findIndex(g => g.TestId == res.testId);
                if (indexTest != -1) {
                  [this.tests[0], this.tests[indexTest]] = [this.tests[indexTest], this.tests[0]];
                }
              }
            });

            this.getTasksSubscription = this.dataStorage.getAllTasks(this.token).subscribe((ppl) => {
              this.tasks.push(new TaskCombo());
              ppl.forEach(element => {
                let ic = new TaskCombo;
                ic.TaskId = element.taskId;
                ic.TaskName = element.taskName;
                this.tasks.push(ic);
              });
            }, () => { }, () => {
              if (res != null) {
                let indexTask = this.tasks.findIndex(g => g.TaskId == res.taskId);
                if (indexTask != -1) {
                  [this.tasks[0], this.tasks[indexTask]] = [this.tasks[indexTask], this.tasks[0]];
                }
              }
            });
          }
        });
      });

    }
  }

  ngOnDestroy(): void {
    if (this.getFeedbakSubscription != null) this.getFeedbakSubscription.unsubscribe();
    if (this.getPersonSubscription != null) this.getPersonSubscription.unsubscribe();
    if (this.addEditSubscription != null) this.addEditSubscription.unsubscribe();
  }

  onSignUpSubmit() {
    let feedback = new Feedback();
    feedback.content = this.content;
    feedback.personSenderId = this.person.personId;
    feedback.personReceiverId = this.currentPersonId;
    feedback.challangeId = this.currentChallengeId == -1 ? null : this.currentChallengeId;
    feedback.taskId = this.currentTaskId == -1 ? null : this.currentTaskId;
    feedback.testId = this.currentTestId == -1 ? null : this.currentTestId;
    feedback.grade = this.grade;
    if (this.operation == "Add") {
      this.addEditSubscription = this.dataStorage.addFeedback(this.token, feedback).subscribe(() => {
        this.toastr.success("Feedback was added succesfully!");
        this.dialog.close();
      }, (error) => {
        this.toastr.error(error.error);
      });
    } else {
      feedback.feedbackId = this.feedbackId;
      this.addEditSubscription = this.dataStorage.editFeedback(this.token, feedback).subscribe(() => {
        this.toastr.success("Feedback was updated succesfully!");
        this.dialog.close();
      }, (error) => {
        this.toastr.error(error.error);
      });
    }
  }

  selectIntern(id: string) {
    this.currentPersonId = +id;
  }

  selectTest(id: string) {
    this.currentTestId = +id;
  }

  selectTask(id: string) {
    this.currentTaskId = +id;
  }

  selectChallenge(id: string) {
    this.currentChallengeId = +id;
  }

}
