import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { LoggedPerson } from 'src/model/loggedperson.model';
import { Question } from 'src/model/question.model';
import { Test } from 'src/model/test.model';
import { DataStorageService } from 'src/services/data-storage.service';
import { UtilsService } from 'src/services/utils.service';

@Component({
  selector: 'app-edit-tests',
  templateUrl: './edit-tests.component.html',
  styleUrls: ['./edit-tests.component.css'],
})
export class EditTestsComponent implements OnInit, OnDestroy {
  @Input() dates = {
    testName: '',
    deadline: moment(new Date()).format('YYYY-MM-DD'),
  };

  testSub!: Subscription;
  isEditMode!: Subscription;
  trainerSub!: Subscription;
  unseledtedQuestionsSub!: Subscription;
  saveSub!: Subscription;
  saveQuestionSub!: Subscription;
  test!: Test | null;
  panelOpenState = false;
  editMode: boolean = false;
  questionsSelected: Question[] = [];
  questionsUnselected: Question[] = [];
  private token!: string;
  trainerId = -1;


  constructor(
    private utils: UtilsService,
    private dataService: DataStorageService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<EditTestsComponent>
  ) {}

  ngOnInit(): void {
    this.utils.initializeError();
    let id=-1;
    this.testSub = this.utils.testToEdit.subscribe((data) => {
      this.test = data;
      if (data != null) {
        this.dates.deadline = moment(data.deadline).format('YYYY-MM-DD');
        this.dates.testName = data.testName;
        this.questionsSelected = data.questions;
        id=data.testId;
      } else {
        this.dates.deadline = moment(new Date()).format('YYYY-MM-DD');
        this.dates.testName = '';
        this.questionsSelected = [];
      }
      this.isEditMode = this.utils.isEditModeForTest.subscribe((data) => {
        this.editMode = data;
      });
    });

    const personString = localStorage.getItem('person');
    if (!personString) {
      return;
    } else {
      const person: LoggedPerson = JSON.parse(personString);
      this.token = person.token;
      this.trainerSub = this.dataService
        .getPerson(person.username, this.token)
        .subscribe((data) => {
          this.trainerId = data.personId;
          console.log("Edit "+ this.token);
        });
        this.unseledtedQuestionsSub = this.dataService.unselectedQuestions(this.token,id).subscribe((data)=>{
          this.questionsUnselected = data;
        });

    }
  }

  ngOnDestroy(): void {
    if (this.testSub) this.testSub.unsubscribe();
    if (this.isEditMode) this.isEditMode.unsubscribe();
    if (this.saveSub) this.saveSub.unsubscribe();
    if (this.saveQuestionSub) this.saveQuestionSub.unsubscribe();
    if (this.unseledtedQuestionsSub) this.unseledtedQuestionsSub.unsubscribe();
  }
  onSignUpSubmit(form: NgForm) {
    let test = new Test();
    test.testName = form.value.testName;
    test.deadline = form.value.deadline;
    test.trainerId=this.trainerId;
    let ids='';
    this.questionsSelected.forEach(element => {
      ids+=element.questionId.toString()+"_";
    });
    if (this.test != null) {
      test.testId = this.test.testId;
      test.dateOfPost = this.test.dateOfPost;
      this.saveSub=this.dataService.updateTest(this.token,test).subscribe( () => {
        this.saveQuestionSub=this.dataService.saveSelectedQuestion(this.token,test.testId,ids).subscribe(() => {
          this.toastr.success("The operations is done succesfully!");
          this.dialogRef.close();
         },
         (error) => {
           this.toastr.error(error.error);
         });
      },
      (error) => {
        this.toastr.error(error.error);
      }
    );
    }else{
      test.dateOfPost=new Date();
      this.saveSub=this.dataService.addTest(this.token,test).subscribe( (data) => {
        this.saveQuestionSub=this.dataService.saveSelectedQuestion(this.token,data.testId,ids).subscribe(() => {
          this.toastr.success("The operations is done succesfully!");
          this.dialogRef.close();
         },
         (error) => {
           this.toastr.error(error.error);
         });
      },
      (error) => {
        this.toastr.error(error.error);
      }
    );
    }


  }

  remove(id:number){
    this.panelOpenState = false;
    let index=this.questionsSelected.findIndex(u=>u.questionId==id);
    this.questionsUnselected.push(this.questionsSelected[index]);

    this.questionsSelected.splice(index,1);
  }

  add(id:number){
    this.panelOpenState = false;
    let index=this.questionsUnselected.findIndex(u=>u.questionId==id);
    this.questionsSelected.push(this.questionsUnselected[index]);
    this.questionsUnselected.splice(index,1);
  }
}
