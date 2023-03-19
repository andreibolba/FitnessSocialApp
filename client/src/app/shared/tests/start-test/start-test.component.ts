import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { Answer } from 'src/model/answer.model';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Question } from 'src/model/question.model';
import { Test } from 'src/model/test.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-start-test',
  templateUrl: './start-test.component.html',
  styleUrls: ['./start-test.component.css'],
})
export class StartTestComponent implements OnInit, OnDestroy {
  testToStartSub!: Subscription;
  peopleSub!: Subscription;
  finishSub!: Subscription;
  resultsSub!: Subscription;
  isVisibleNext = false;
  isVisiblePrev = false;
  isVisibleFinish = false;
  private token: string = '';
  private internId: number = -1;
  private answers: Answer[] = [];
  results: Answer[] = [];

  opA: string = '';
  opB: string = '';
  opC: string = '';
  opD: string = '';
  opE: string = '';
  opF: string = '';

  actualQuestion: Question = new Question();
  actualQuestionIndex: number = -1;

  test: Test = new Test();

  constructor(
    private utils: UtilsService,
    private data: DataStorageService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<StartTestComponent>
  ) {}

  answer(op: string) {
    if (op == this.actualQuestion.correctOption) {
      this.toastr.success('Correct answer!');
      this.correctAnswer(op);
    } else {
      this.toastr.error('Wrong answer!');
      this.correctAnswer(this.actualQuestion.correctOption);
      this.wrongAnswer(op);
    }

    this.answers.unshift(
      new Answer(this.actualQuestion.questionId, new Question(), op)
    );

    if (this.actualQuestionIndex != this.test.questions.length - 1)
      this.isVisibleNext = true;
    else this.isVisibleFinish = true;
  }

  nextQuestion() {
    if (this.results.length == 0) {
      if (this.actualQuestionIndex != this.test.questions.length - 1) {
        this.actualQuestionIndex++;
        this.actualQuestion = this.test.questions[this.actualQuestionIndex];
      }
      this.isVisibleNext = false;
      this.initializeAnswers();
    } else {
      if (this.actualQuestionIndex != this.test.questions.length - 2) {
        this.isVisibleNext = true;
      } else {
        this.isVisibleFinish = true;
        this.isVisibleNext = false;
      }
      this.actualQuestionIndex++;
      this.actualQuestion = this.test.questions[this.actualQuestionIndex];
      this.initializeAnswers();
      if (
        this.results[this.actualQuestionIndex].internOption ==
        this.actualQuestion.correctOption
      )
        this.correctAnswer(this.results[this.actualQuestionIndex].internOption);
      else {
        this.wrongAnswer(this.results[this.actualQuestionIndex].internOption);
        this.correctAnswer(this.actualQuestion.correctOption);
      }
      if (this.actualQuestionIndex != 0) this.isVisiblePrev = true;
    }
  }

  prevQuestion() {
    this.actualQuestionIndex--;
    this.actualQuestion = this.test.questions[this.actualQuestionIndex];
    this.initializeAnswers();
    if (
      this.results[this.actualQuestionIndex].internOption ==
      this.actualQuestion.correctOption
    )
      this.correctAnswer(this.results[this.actualQuestionIndex].internOption);
    else {
      this.wrongAnswer(this.results[this.actualQuestionIndex].internOption);
      this.correctAnswer(this.actualQuestion.correctOption);
    }
    if (this.actualQuestionIndex == 0){
      this.isVisiblePrev = false;
      this.isVisibleNext = true;
    }
    if(this.actualQuestionIndex!=this.test.questions.length - 1)
    this.isVisibleNext=true;
  }

  finishTest() {
    if (this.results.length == 0) {
      console.log(this.answers);
      this.finishSub = this.data
        .sendTest(this.token, this.internId, this.test.testId, this.answers)
        .subscribe(
          () => {
            this.toastr.success('You finished the test!');
          },
          (error) => {
            this.toastr.error(error.error);
            console.log(error);
          }
        );
    }

    this.dialogRef.close();
  }

  ngOnDestroy(): void {
    if (this.testToStartSub != null) this.testToStartSub.unsubscribe();
    if (this.peopleSub != null) this.peopleSub.unsubscribe();
  }
  ngOnInit(): void {
    this.utils.initializeError();
    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.peopleSub = this.data
        .getPerson(person.username, person.token)
        .subscribe((data) => {
          if (data) this.internId = data.personId;
          this.testToStartSub = this.utils.testToStart.subscribe((res) => {
            if (res) {
              this.test = res;
              this.actualQuestion = this.test.questions[0];
              this.actualQuestionIndex = 0;
              this.initializeAnswers();
            }
            this.resultsSub = this.data
              .getResult(this.token, this.internId, this.test.testId)
              .subscribe((data) => {
                this.results = data;
                if (this.results.length == 0) this.isVisibleNext = false;
                else {
                  this.isVisibleNext = true;
                  if (data[0].internOption == this.actualQuestion.correctOption)
                    this.correctAnswer(data[0].internOption);
                  else {
                    this.wrongAnswer(data[0].internOption);
                    this.correctAnswer(this.actualQuestion.correctOption);
                  }
                }
              });
          });
        });
    }
  }

  private initializeAnswers() {
    this.opA = 'answer';
    this.opB = 'answer';
    this.opC = 'answer';
    this.opD = 'answer';
    this.opE = 'answer';
    this.opF = 'answer';
  }

  private correctAnswer(op: string) {
    switch (op) {
      case 'A':
        this.opA = 'answer correct';
        return;
      case 'B':
        this.opB = 'answer correct';
        return;
      case 'C':
        this.opC = 'answer correct';
        return;
      case 'D':
        this.opD = 'answer correct';
        return;
      case 'E':
        this.opE = 'answer correct';
        return;
      case 'F':
        this.opF = 'answer correct';
        return;
    }
  }

  private wrongAnswer(op: string) {
    switch (op) {
      case 'A':
        this.opA = 'answer wrong';
        return;
      case 'B':
        this.opB = 'answer wrong';
        return;
      case 'C':
        this.opC = 'answer wrong';
        return;
      case 'D':
        this.opD = 'answer wrong';
        return;
      case 'E':
        this.opE = 'answer wrong';
        return;
      case 'F':
        this.opF = 'answer wrong';
        return;
    }
  }
}
